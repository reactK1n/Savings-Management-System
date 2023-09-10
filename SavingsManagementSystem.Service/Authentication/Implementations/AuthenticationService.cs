using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using SavingsManagementSystem.Common.CustomExceptions;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Model;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.Mail.Interfaces;
using System.Net;
using System.Security.Claims;

namespace SavingsManagementSystem.Service.Authentication.Implementations
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _token;
		private readonly IMailService _mailService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IUnitOfWork _unit;
		private readonly IOTPService _otpService;
		private readonly IConfiguration _config;
		private readonly GenerateLink _generateLink;

		public AuthenticationService(UserManager<ApplicationUser> userManager,
			ITokenService token,
			IMailService mailService,
			IHttpContextAccessor httpContextAccessor,
			IUnitOfWork unit,
			IOTPService otpService,
			IConfiguration config,
			GenerateLink generateLink)
		{
			_userManager = userManager;
			_token = token;
			_mailService = mailService;
			_httpContextAccessor = httpContextAccessor;
			_unit = unit;
			_otpService = otpService;
			_config = config;
			_generateLink = generateLink;
		}

		public async Task<RegistrationResponse> Register(ApplicationUser user, string password, UserRole role)
		{
			var results = await _userManager.CreateAsync(user, password);
			if (!results.Succeeded)
			{
				var errors = string.Empty;
				foreach (var error in results.Errors)
				{
					errors = error.Description + Environment.NewLine;
				}
				throw new MissingFieldException(errors);
			}
			await _userManager.AddToRoleAsync(user, role.ToString());

			var response = new RegistrationResponse
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Username = user.UserName

			};

			return response;
		}

		public async Task<LoginResponse> Login(LoginRequest loginRequest)
		{
			var user = await _userManager.FindByEmailAsync(loginRequest.Email);
			if (user == null)
			{
				throw new ArgumentNullException("Email Provided is Invalid");
			}
			var isPasswordMatch = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
			if (!isPasswordMatch)
			{
				throw new ArgumentNullException("Password Not Match");
			}
			var token = await _token.GetToken(user);

			var response = new LoginResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Username = user.UserName,
				Token = token
			};

			return response;
		}

		public async Task<string> ForgetPasswordAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new ArgumentNullException($"Email {email} provided does not exist in our Database");
			};

			var otp = await _otpService.CreateOtpAsync(user.Id, 30);
			await _unit.SaveChangesAsync();

			// Load the email template from the file
			var htmlPath = Path.Combine("StaticFiles", "Html", "ForgetPassword.html");
			var emailTemplate = File.ReadAllText(htmlPath);
			var queryParams = new ForgetPassQueryParams
			{
				UserId = user.Id,
				OtpId = otp.Id
			};
			var resetLink = _generateLink.GenerateUrl("ResetPassword", "Auth", queryParams);
			// Replacing the {{RESET_LINK}} placeholder with the actual reset link
			emailTemplate = emailTemplate.Replace("{{RESET_LINK}}", resetLink);
			var mailRequest = new MailRequest()
			{
				Subject = "Reset Password",
				RecipientEmail = email,
				Body = emailTemplate

			};
			var result = await _mailService.SendEmailAsync(mailRequest);
			if (!result)
			{
				return "Email not Successful";
			}
			return "Sent Successfully";
		}

		public async Task<string> ConfirmEmailAsync(ConfirmEmailRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);
			if (user == null)
			{
				throw new ArgumentNullException("Invalid User Id");
			}
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			if (token == null)
			{
				throw new ArgumentNullException("Invalid Token Provided");
			}
			var otp = await _unit.OTP.FetchByOtpIdAsync(request.OtpId);
			var isExpired = otp.Expire >= DateTime.UtcNow;
			if (isExpired)
			{
				throw new OTPExpiredException("The OTP has expired.");
			}
			if (otp.IsUsed)
			{
				throw new InvalidOperationException("Link Has been Used");
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);
			var errors = string.Empty;
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					errors += error + Environment.NewLine;
				}
				throw new InvalidOperationException(errors);
			}

			otp.IsUsed = true;
			_unit.OTP.Update(otp);
			await _unit.SaveChangesAsync();

			return "Email Confirmed Successfully";
		}

		public async Task<string> ChangePasswordAsync(ChangePasswordRequest request)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentNullException("Invalid Id Provided");
			}
			var isPasswordMatch = await _userManager.CheckPasswordAsync(user, request.OldPassword);
			if (!isPasswordMatch)
			{
				throw new InvalidOperationException("Provided Password is not Correct, make sure your Old Password is valid");
			}
			await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

			return "Password Changed Successfully";
		}

		public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);
			if (user == null)
			{
				throw new ArgumentNullException("user not Found");
			}

			var otp = await _unit.OTP.FetchByOtpIdAsync(request.OtpId);
			var isExpired = otp.Expire >= DateTime.UtcNow;
			if (isExpired)
			{
				throw new OTPExpiredException("The OTP has expired.");
			}
			if (otp.IsUsed)
			{
				throw new InvalidOperationException("Link Has been Used");
			}

			var isPasswordMatch = await _userManager.CheckPasswordAsync(user, request.Password);
			if (isPasswordMatch)
			{
				throw new ArgumentNullException("you can not use your old password");
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
			var errors = string.Empty;
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					errors += error + Environment.NewLine;
				}
				throw new InvalidOperationException(errors);
			}
			otp.IsUsed = true;
			_unit.OTP.Update(otp);
			await _unit.SaveChangesAsync();

			return "Password Changed Successfully";
		}
	}
}
