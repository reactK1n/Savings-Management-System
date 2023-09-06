using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
	{
		public ChangePasswordRequestValidator()
		{
			RuleFor(user => user.OldPassword).PassWord();
			RuleFor(user => user.NewPassword).PassWord();
		}
	}
}
