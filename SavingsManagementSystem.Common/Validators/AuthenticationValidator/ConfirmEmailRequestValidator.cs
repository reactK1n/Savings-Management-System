using FluentValidation;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
	{
		public ConfirmEmailRequestValidator() 
		{
			RuleFor(user => user.OtpId).NotEmpty().NotNull();
			RuleFor(user => user.Token).NotNull().NotEmpty();
		}
	}
}
