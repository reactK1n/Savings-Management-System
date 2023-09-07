using FluentValidation;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Common.Validators.AuthenticationValidator
{
	public class MailRequestValidator : AbstractValidator<MailRequest>
	{
		public MailRequestValidator()
		{
			RuleFor(email => email.Subject).NotEmpty().NotNull();
			RuleFor(email => email.RecipientEmail).EmailAddress();
		}
	}
}
