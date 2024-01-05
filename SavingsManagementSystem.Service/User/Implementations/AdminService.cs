﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.Files.Interfaces;
using SavingsManagementSystem.Service.Mail.Interfaces;
using SavingsManagementSystem.Service.User.Interfaces;
using System.Security.Claims;

namespace SavingsManagementSystem.Service.User.Implementations
{
	public class AdminService : IAdminService
	{

		private readonly IAuthenticationService _auth;
		private readonly IUnitOfWork _unit;
		private readonly IVerificationTokenService _vTokenService;
		private readonly IMailService _mailService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IImageService _image;

		public AdminService(IAuthenticationService auth,
			IUnitOfWork unit,
			IVerificationTokenService vTokenService,
			IMailService mailService,
			UserManager<ApplicationUser> userManager,
			IHttpContextAccessor httpContextAccessor,
			IImageService image)
		{
			_unit = unit;
			_auth = auth;
			_vTokenService = vTokenService;
			_mailService = mailService;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_image = image;
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


		public async Task UpdateUserAsync(UpdateRequest request)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentNullException("User not found");
			}
			var address = await _unit.Address.FetchAsync(user.AddressId);

			//assigning values
			user.FirstName = !string.IsNullOrEmpty(request.FirstName) ? request.FirstName : user.FirstName;
			user.LastName = !string.IsNullOrEmpty(request.LastName) ? request.LastName : user.LastName;
			user.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : user.Email;
			user.UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : user.UserName;
			user.ImageUri = request.Image != null ? await _image.UploadImageAsync(request.Image) : user.ImageUri;
			address.State = !string.IsNullOrEmpty(request.State) ? request.State : address.State;
			address.City = !string.IsNullOrEmpty(request.City) ? request.City : address.City;

			//updating entities
			_unit.Address.Update(address);
			var result = await _userManager.UpdateAsync(user);

			//saving changes
			await _unit.SaveChangesAsync();

			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += error.Description + Environment.NewLine;
				};
				throw new MissingFieldException(errors);
			}
		}

		public async Task DeleteUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentNullException($"User with {userId} Not Found ");
			};

			var member = await _unit.Member.FetchByUserIdAsync(user.Id);
			var address = await _unit.Address.FetchAsync(user.AddressId);

			_unit.Member.Delete(member);
			_unit.Address.Delete(address);
			var result = await _userManager.DeleteAsync(user);
			await _unit.SaveChangesAsync();

			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += error.Description + Environment.NewLine;
				};
				throw new MissingFieldException(errors);
			}
		}

	}
}
