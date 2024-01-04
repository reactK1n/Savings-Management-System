using FluentValidation;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class RefreshTokenRequestValidation : AbstractValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidation()
		{
			RuleFor(user => user.UserId).NotEmpty().NotNull();
			RuleFor(user => user.RefreshToken).NotNull().NotEmpty();
		}

	}
}
