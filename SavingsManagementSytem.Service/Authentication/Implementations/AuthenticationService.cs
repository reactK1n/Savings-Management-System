using Microsoft.AspNetCore.Identity;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using System.Net;
using SavingsManagementSystem.Service.Mail.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SavingsManagementSystem.Service.Authentication.Implementations
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _token;
		private readonly IMailService _mailService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthenticationService(UserManager<ApplicationUser> userManager, ITokenService token, IMailService mailService, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_token = token;
			_mailService = mailService;
			_httpContextAccessor = httpContextAccessor;
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

		public async Task<string> ForgetPassword(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				throw new ArgumentNullException($"Email {email} provided does not exist in our Database");
			};
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var encodeToken = TokenConverter.EncodeToken(token);
			var userRole = await _userManager.GetRolesAsync(user);

			//var mailBody = await EmailBodyBuilder.GetEmailBody(user, userRole.ToList(), emailTempPath: "StaticFiles/Html/ForgotPassword.html", linkName: "ResetPassword", encodedToken, controllerName: "Auth");


			var mailRequest = new MailResquest()
			{
				Subject = "Reset Password",
				//Body = mailBody,
				RecipientEmail = email
			};
			var response = await _mailService.SendEmailAsync(mailRequest);
			return response;
		}

		public async Task<string> ConfirmEmail(ConfirmEmailRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
			{
				throw new ArgumentNullException("Email {request.Email} Provided is Invalid ");
			}

			var decodedToken = TokenConverter.DecodeToken(request.Token);
			if (decodedToken == null)
			{
				throw new ArgumentNullException("Invalid Token Provided");
			}
			var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
			var errors = string.Empty;
			if (!result.Succeeded)
			{
                foreach (var error in result.Errors)
                {
					errors += error + Environment.NewLine;
                }
				throw new InvalidOperationException(errors);
            }

			return "Email Confirmed Successfully";
		}

		public async Task<string> ChangePassword(ChangePasswordRequest request)
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
				throw new Exception("Provided Password is not Correct, Check your Old Password");
            }
            await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

			return "Password Changed Successfully";
        }
	}
}
