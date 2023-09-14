using FluentValidation;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
	{
		public ConfirmEmailRequestValidator() 
		{
			RuleFor(user => user.VToken).NotEmpty().NotNull();
			RuleFor(user => user.UserId).NotNull().NotEmpty();
		}
	}
}
