using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.Mail.Interfaces;

namespace SavingsManagementSystem.Service.Mail.Implementations
{
	public class MailService : IMailService
	{
		public async Task<string> SendEmailAsync(MailResquest resquest)
		{
			var response = "Email Sent Successfully";
			return response;
		}
	}
}
