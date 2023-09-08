using Microsoft.AspNetCore.Http;

namespace SavingsManagementSystem.Common.DTOs
{
	public class MailRequest
	{
        public string Subject { get; set; }

		public string Body { get; set; }

        public string RecipientEmail { get; set; }

	}
}
