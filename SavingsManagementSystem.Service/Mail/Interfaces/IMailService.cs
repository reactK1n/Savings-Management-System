using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.Mail.Interfaces
{
	public interface IMailService
	{
		Task<bool> SendEmailAsync(MailRequest mailRequest);
	}
}
