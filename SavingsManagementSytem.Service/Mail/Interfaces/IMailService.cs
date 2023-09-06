using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.Mail.Interfaces
{
	public interface IMailService
	{
		Task<string> SendMailAsync(MailResquest resquest);
	}
}
