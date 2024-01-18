using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator() 
		{
			RuleFor(user => user.Email).EmailAddress();
			RuleFor(user => user.Password).PassWord();
		}
	}
}
