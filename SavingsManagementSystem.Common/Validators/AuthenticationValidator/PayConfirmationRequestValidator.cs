using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class PayConfirmationRequestValidator : AbstractValidator<PayConfirmationRequest>
	{
        public PayConfirmationRequestValidator()
        {
            RuleFor(user => user.SessionId).NotNull().NotEmpty();
            RuleFor(user => user.Amount.ToString()).IsDigit();    
        }
    }
}
