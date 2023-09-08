using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.Mail.Interfaces;

namespace SavingsManagementSystem.Service.Mail.Implementations
{
	public class MailService : IMailService
	{
		private readonly IConfiguration _config;

		public MailService(IConfiguration config)
		{
			_config = config;
		}
		public async Task<bool> SendEmailAsync(MailRequest mailRequest)
		{
			string apiKey = _config["MailJetSettings:PublicKey"];
			string apiSecret = _config["MailJetSettings:PrivateKey"];

			MailjetClient client = new MailjetClient(apiKey, apiSecret);

			MailjetRequest request = new MailjetRequest
			{
				Resource = Send.Resource,
			}
				.Property(Send.FromEmail, "upskillz.org@gmail.com")
				.Property(Send.FromName, "octalTech")
				.Property(Send.Subject, mailRequest.Subject)
				.Property(Send.HtmlPart, mailRequest.Body)
				.Property(Send.Recipients, new JArray {
				new JObject {
					{"Email", mailRequest.RecipientEmail}
				}
				});

			MailjetResponse response = await client.PostAsync(request);
			return response != null;
		}

	}
}
