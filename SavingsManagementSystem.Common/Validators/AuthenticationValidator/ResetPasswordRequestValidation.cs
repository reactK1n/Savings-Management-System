using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidation()
		{
			RuleFor(user => user.Password).PassWord();
		}
	}
}
