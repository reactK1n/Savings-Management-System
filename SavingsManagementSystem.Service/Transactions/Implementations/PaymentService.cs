using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Mail.Interfaces;
using SavingsManagementSystem.Service.Transactions.Interfaces;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace SavingsManagementSystem.Service.Transactions.Implementations
{
	public class PaymentService : IPaymentService

	{
		private readonly IUnitOfWork _unit;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<ApplicationUser> _user;
		private readonly IMailService _mailService;
		private readonly IConfiguration _config;

		public PaymentService(
			IUnitOfWork unit,
			IHttpContextAccessor httpContextAccessor,
			UserManager<ApplicationUser> user,
			IMailService mailService,
			IConfiguration config)
		{
			_config = config;
			StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
			_unit = unit;
			_httpContextAccessor = httpContextAccessor;
			_user = user;
			_mailService = mailService;
		}
		public async Task<InitiatePaymentResponse> InitiatePaymentSessionAsync([FromBody] PaymentRequest model)
		{

			var currency = "usd";
			var successUrl = "https://localhost:7210";
			var cancelUrl = "http://localhost:5036";

			var option = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>
					{
						new SessionLineItemOptions
						{
							PriceData = new SessionLineItemPriceDataOptions
							{
								Currency = currency,
								UnitAmount = (long)model.Amount * 100,  // Assume Amount is in dollars
                                ProductData = new SessionLineItemPriceDataProductDataOptions
								{
									Name = model.ProductName,
									Description = model.ProductDescription
								},
							},
							Quantity = 1
						}
					},
				Mode = "payment",
				SuccessUrl = successUrl,
				CancelUrl = cancelUrl
			};

			var service = new SessionService();
			var session = service.Create(option);

			var response = new InitiatePaymentResponse
			{

				SessionId = session.Id,
				SessionUrl = session.Url,
				SessionStatus = session.Status,
				SessionAmount = (long)session.AmountSubtotal / 100
			};

			return response;
		}


		public async Task PaymentConfirmationAsync(PayConfirmationRequest request)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
			var user = await _user.FindByIdAsync(userId) ?? throw new ArgumentNullException($"User with {userId} does not exist");
			var member = await _unit.Member.FetchByUserIdAsync(userId) ?? throw new ArgumentNullException($"Member with {userId} does not exist");
			var generatedReference = string.Empty;
			var checkoutStatus = await PaymentCheckOutStatusAsync(request.SessionId);
			var paymentIntentId = await GetPaymentIntentIdAsync(request.SessionId);
			var receiptUrl = await PaymentReceiptUrlAsync(paymentIntentId);

			while (true)
			{

				generatedReference = Helper.GenerateReference();
				var transaction = _unit.Transaction.Fetch().FirstOrDefault(tr => tr.Reference == generatedReference);
				if (transaction == null)
				{
					break;
				}

			}

			if (checkoutStatus == "open")
			{
				throw new Exception("payment Not Completed");
			}

			if (checkoutStatus == "complete")
			{

				var transaction = new Transaction()
				{
					MemberId = member.Id,
					TransactionType = "Savings",
					Description = $"{user.FirstName} {user.LastName} saved money into is Account",
					Amount = request.Amount,
					Reference = generatedReference,
					Member = member,
				};

				// Load the email template from the file
				var htmlPath = Path.Combine("StaticFiles", "Html", "PaymentReceipt.html");
				var emailTemplate = System.IO.File.ReadAllText(htmlPath);
				// Replacing the {{INVITE_LINK}} placeholder with the actual reset link
				emailTemplate = emailTemplate.Replace("{SAVING_AMOUNT}", request.Amount.ToString()).Replace("{{RECEIPT_URL}}", receiptUrl);
				var mailRequest = new MailRequest()
				{
					Subject = "Payment Receipt",
					RecipientEmail = user.Email,
					Body = emailTemplate
				};

				//await _mailService.SendEmailAsync(mailRequest);
				await _unit.Transaction.Create(transaction);
				await _unit.SaveChangesAsync();
			}
		}



		public async Task<string> PaymentReceiptUrlAsync(string paymentIntentId)
		{
			var paymentIntentStatus = await PaymentIntentStatusAsync(paymentIntentId) ?? throw new ArgumentNullException();

			if (paymentIntentStatus == "succeeded")
			{
				var chargeListOptions = new ChargeListOptions
				{
					PaymentIntent = paymentIntentId,
					Limit = 1 // Assuming you want only one charge (latest charge)
				};

				var chargeService = new ChargeService();
				var charges = chargeService.List(chargeListOptions);

				var receiptUrl = charges.FirstOrDefault()?.ReceiptUrl;
				return receiptUrl;
			}
			return null;
		}

		public async Task<string> PaymentCheckOutStatusAsync(string sessionId)
		{
			var service = new SessionService();
			var checkoutSession = service.Get(sessionId);
			return checkoutSession.Status ?? throw new StripeException($"payment Checkout Session with {sessionId} does not exist");
		}

		public async Task<string> PaymentIntentStatusAsync(string paymentIntentId)
		{
			var service = new PaymentIntentService();
			var paymentIntent = service.Get(paymentIntentId);
			return paymentIntent.Status ?? throw new StripeException($"Payment Intent with {paymentIntentId} does not exist");
		}

		public async Task<string> GetPaymentIntentIdAsync(string sessionId)
		{
			var service = new SessionService();
			var checkoutSession = service.Get(sessionId);
			return checkoutSession.PaymentIntentId ?? throw new StripeException($"Checkout Session with {sessionId} does not exist");
		}
	}
}

