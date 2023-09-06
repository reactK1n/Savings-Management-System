using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.Mail.Interfaces;

namespace SavingsManagementSystem.Service.Mail.Implementations
{
	public class MailService : IMailService
	{
		public async Task<string> SendMailAsync(MailResquest resquest)
		{
			var response = "Email Sent Successfully";
			return response;
		}
	}
}
