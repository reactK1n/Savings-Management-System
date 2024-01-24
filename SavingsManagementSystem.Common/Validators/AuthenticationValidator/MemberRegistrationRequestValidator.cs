using FluentValidation;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.ValidatorSettings;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class MemberRegistrationRequestValidator : AbstractValidator<MemberRegistrationRequest>
	{
		public MemberRegistrationRequestValidator()
		{
			RuleFor(user => user.FirstName).HumanName();
			RuleFor(user => user.LastName).HumanName();
			RuleFor(user => user.Email).EmailAddress();
			RuleFor(user => user.Username).MinimumLength(1);
			RuleFor(user => user.PhoneNumber).PhoneNumber();
			RuleFor(user => user.Password).PassWord();
			RuleFor(user => user.Token).NotEmpty().NotNull();
		}
	}
}

