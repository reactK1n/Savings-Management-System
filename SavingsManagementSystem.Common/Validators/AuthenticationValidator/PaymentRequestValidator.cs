using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
	{
		public PaymentRequestValidator()
		{

			RuleFor(pay => pay.Amount.ToString()).IsDigit();//still coming back for this

			RuleFor(pay => pay.ProductName).NotEmpty().NotNull();

			RuleFor(pay => pay.ProductDescription).NotNull().NotEmpty();
		}
	}
}
