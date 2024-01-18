using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			RuleFor(user => user.Password).PassWord();

			RuleFor(user => user.UserId).NotEmpty().NotNull();

			RuleFor(user => user.VToken).NotNull().NotEmpty();

		}
	}
}
