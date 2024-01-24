using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.Transactions.Interfaces;
using Stripe;

namespace SavingsManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}



		[HttpPost]
		[Authorize(Policy = "Member")]
		[Route("Initiate-Payment")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest request)
		{
			try
			{
				var response = await _paymentService.InitiatePaymentSessionAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();

			}

			catch (StripeException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}



		[HttpPost]
		[Authorize(Policy = "Member")]
		[Route("Payment-Confirmation")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> PaymentConfirmation([FromBody] PayConfirmationRequest request)
		{
			try
			{
				await _paymentService.PaymentConfirmationAsync(request);
				return NoContent();
			}

			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (StripeException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
