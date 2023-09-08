using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.Mail.Interfaces
{
	public interface IMailService
	{
		Task<bool> SendEmailAsync(MailRequest resquest, string htmlFilePath);
	}
}
