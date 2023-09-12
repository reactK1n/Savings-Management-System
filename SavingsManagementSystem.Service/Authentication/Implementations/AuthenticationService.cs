using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
		private readonly IVerificationTokenService _vTokenService;
		private readonly IConfiguration _config;
		private readonly GenerateLink _generateLink;

		public AuthenticationService(UserManager<ApplicationUser> userManager,
			ITokenService token,
			IMailService mailService,
			IHttpContextAccessor httpContextAccessor,
			IUnitOfWork unit,
			IConfiguration config,
			GenerateLink generateLink,
			IVerificationTokenService vTokenService)
		{
			_userManager = userManager;
			_token = token;
			_mailService = mailService;
			_httpContextAccessor = httpContextAccessor;
			_unit = unit;
			_config = config;
			_generateLink = generateLink;
			_vTokenService = vTokenService;
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
			var refreshToken = _token.GenerateRefreshToken();
			user.RefreshToken = refreshToken;
			user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); //sets refresh token for 7 days
			//updating our db
			await _userManager.UpdateAsync(user);
			await _unit.SaveChangesAsync();

			var response = new LoginResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Username = user.UserName,
				Token = token,
				RefreshToken = refreshToken
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

			var vToken = await _vTokenService.CreateVerificationTokenAsync(user.Id, 30);
			await _unit.SaveChangesAsync();

			// Load the email template from the file
			var htmlPath = Path.Combine("StaticFiles", "Html", "ForgetPassword.html");
			var emailTemplate = File.ReadAllText(htmlPath);
			var queryParams = new ForgetPassQueryParams
			{
				UserId = user.Id,
				Token = vToken.Token
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
				throw new ArgumentNullException("Invalid User Provided");
			}
			var vToken = await _unit.VerificationToken.FetchByTokenAsync(request.vToken);
			var isExpired = vToken.ExpiryTime >= DateTime.UtcNow;
			if (isExpired)
			{
				throw new OTPExpiredException("The Link has expired.");
			}
			if (vToken.IsUsed)
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

			vToken.IsUsed = true;
			_unit.VerificationToken.Update(vToken);
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

			var vToken = await _unit.VerificationToken.FetchByTokenAsync(request.VToken);
			var isExpired = vToken.ExpiryTime >= DateTime.UtcNow;
			if (isExpired)
			{
				throw new OTPExpiredException("The Link has expired.");
			}
			if (vToken.IsUsed)
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
			vToken.IsUsed = true;
			_unit.VerificationToken.Update(vToken);
			await _unit.SaveChangesAsync();

			return "Password Changed Successfully";
		}
	}
}
