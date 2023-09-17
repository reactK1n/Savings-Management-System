using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface IAuthenticationService
	{
		Task<RegistrationResponse> Register(ApplicationUser user, string password, UserRole role);

		Task<LoginResponse> Login(LoginRequest loginRequest);

		Task<string> ForgetPasswordAsync(string email);

		Task<string> ConfirmEmailAsync(ConfirmEmailRequest request);

		Task<string> ChangePasswordAsync(ChangePasswordRequest request);

		Task<string> ResetPasswordAsync(ResetPasswordRequest request);

		Task VerifyLinkAsync(string token);

	}
}
