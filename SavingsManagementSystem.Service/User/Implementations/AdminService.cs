using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.Mail.Interfaces;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Service.User.Implementations
{
	public class AdminService : IAdminService
	{

		private readonly IAuthenticationService _auth;
		private readonly IUnitOfWork _unit;
		private readonly IVerificationTokenService _vTokenService;
		private readonly IMailService _mailService;

		public AdminService(IAuthenticationService auth,
			IUnitOfWork unit,
			IVerificationTokenService vTokenService,
			IMailService mailService)
		{
			_unit = unit;
			_auth = auth;
			_vTokenService = vTokenService;
			_mailService = mailService;
		}

		public async Task<RegistrationResponse> RegisterAsync(AdminRegistrationRequest request)
		{
			var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				UserName = request.Username,
				EmailConfirmed = true
			};

			var response = await _auth.Register(user, request.Password, UserRole.Admin);
			await _unit.SaveChangesAsync();

			return response;
		}


		public async Task SendMemberInviteAsync(string email)
		{
			//create a verification token for the link
			var expiryTime = DateTime.UtcNow.AddMinutes(30);
			var vToken = await _vTokenService.CreateVerificationTokenAsync(expiryTime, email);
			var encodedToken = TokenConverter.EncodeToken(vToken.Token);
			await _unit.SaveChangesAsync();

			// Load the email template from the file
			var htmlPath = Path.Combine("StaticFiles", "Html", "MemberInvite.html");
			var emailTemplate = File.ReadAllText(htmlPath);
			var queryParams = $"email={email}&token={encodedToken}";
			var inviteLink = LinkGenerator.GenerateUrl("VerifyLink", "Auth", queryParams);

			// Replacing the {{INVITE_LINK}} placeholder with the actual reset link
			emailTemplate = emailTemplate.Replace("{{INVITE_LINK}}", inviteLink);
			var mailRequest = new MailRequest()
			{
				Subject = "Member Invite",
				RecipientEmail = email,
				Body = emailTemplate
			};
			try
			{
				await _mailService.SendEmailAsync(mailRequest);
				vToken.Status = "Successful";
				_unit.VerificationToken.Update(vToken);
				await _unit.SaveChangesAsync();
			}
			catch 
			{
				vToken.Status = "Fail";
				_unit.VerificationToken.Update(vToken);
				await _unit.SaveChangesAsync();
			}
		}

	}
}
