using FluentValidation;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidator()
		{
			RuleFor(user => user.UserId).NotEmpty().NotNull();
			RuleFor(user => user.RefreshToken).NotNull().NotEmpty();
		}

	}
}
