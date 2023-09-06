using FluentValidation;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class MailRequestValidator : AbstractValidator<MailResquest>
	{
		public MailRequestValidator()
		{
			RuleFor(email => email.Subject).NotEmpty().NotNull();
			RuleFor(email => email.Body).NotEmpty().NotNull();
			RuleFor(email => email.RecipientEmail).EmailAddress();
		}
	}
}
