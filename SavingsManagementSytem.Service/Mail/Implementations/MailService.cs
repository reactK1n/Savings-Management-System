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
		public async Task<bool> SendEmailAsync(MailRequest mailrequest, string htmlFilePath)
		{
			string apiKey = _config["MailJetSettings:PublicKey"];
			string apiSecret = _config["MailJetSettings:PrivateKey"];
			string htmlContent = File.ReadAllText(htmlFilePath);


			MailjetClient client = new MailjetClient(apiKey, apiSecret);

			MailjetRequest request = new MailjetRequest
			{
				Resource = Send.Resource,
			}
				.Property(Send.FromEmail, "upskillz.org@gmail.com")
				.Property(Send.FromName, "octalTech")
				.Property(Send.Subject, mailrequest.Subject)
				.Property(Send.TextPart, mailrequest.Body)
				.Property(Send.HtmlPart, htmlContent)
				.Property(Send.Recipients, new JArray {
				new JObject {
					{"Email", mailrequest.RecipientEmail}
				}
				});

			MailjetResponse response = await client.PostAsync(request);
			return response != null;
		}

	}
}
