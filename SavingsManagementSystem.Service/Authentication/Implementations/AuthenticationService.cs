using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SavingsManagementSystem.Common.CustomExceptions;
using SavingsManagementSystem.Common.DTOs;
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

		public AuthenticationService(UserManager<ApplicationUser> userManager,
			ITokenService token,
			IMailService mailService,
			IHttpContextAccessor httpContextAccessor,
			IUnitOfWork unit,
			IConfiguration config,
			IVerificationTokenService vTokenService)
		{
			_userManager = userManager;
			_token = token;
			_mailService = mailService;
			_httpContextAccessor = httpContextAccessor;
			_unit = unit;
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
				throw new Exception(errors);
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

		public async Task ForgetPasswordAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new ArgumentNullException($"Email {email} provided does not exist in our Database");
			};

			var vToken = await _vTokenService.CreateVerificationTokenAsync(30, "Successful", email, user.Id);
			var encodedToken = TokenConverter.EncodeToken(vToken.Token);
			await _unit.SaveChangesAsync();

			// Load the email template from the file
			var htmlPath = Path.Combine("StaticFiles", "Html", "ForgetPassword.html");
			var emailTemplate = File.ReadAllText(htmlPath);
            var queryParams = $"userId={user.Id}&token={encodedToken}"; // Already encoded
			var resetLink = LinkGenerator.GenerateUrl("VerifyLink", "Auth", queryParams);

			// Replacing the {{RESET_LINK}} placeholder with the actual reset link
			emailTemplate = emailTemplate.Replace("{{RESET_LINK}}", resetLink);
			var mailRequest = new MailRequest()
			{
				Subject = "Reset Password",
				RecipientEmail = email,
				Body = emailTemplate

			};
			try
			{
				await _mailService.SendEmailAsync(mailRequest);
			}
			catch
			{
				vToken.Status = "Fail";
				_unit.VerificationToken.Update(vToken);
				await _unit.SaveChangesAsync();
			}
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

			var decodedvToken = TokenConverter.DecodeToken(request.VToken);
			var vToken = await _unit.VerificationToken.FetchByTokenAsync(decodedvToken);
			if (vToken == null)
			{
				throw new ArgumentNullException("Invalid Verification Token Provided");
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
			var userId =  _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
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

			var decodedvToken = TokenConverter.DecodeToken(request.VToken);
			var vToken = await _unit.VerificationToken.FetchByTokenAsync(decodedvToken);
			if (vToken == null)
			{
				throw new ArgumentNullException("user token is Invalid");
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

		public async Task VerifyLinkAsync(string token)
		{
			var decodedvToken = TokenConverter.DecodeToken(token);
			var vToken = await _unit.VerificationToken.FetchByTokenAsync(decodedvToken);
			if (vToken == null)
			{
				throw new ArgumentNullException("Invalid verification token provided");
			}

			var isExpired = vToken.ExpiryTime < DateTime.UtcNow;
			if (isExpired)
			{
				throw new LinkExpiredException("The Link has expired.");
			}
			if (vToken.IsUsed)
			{
				throw new InvalidOperationException("Link Has been Used");
			}
		}
	}
}
