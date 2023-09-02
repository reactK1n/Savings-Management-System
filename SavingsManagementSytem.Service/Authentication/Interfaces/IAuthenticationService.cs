using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface IAuthenticationService
	{
		Task<RegistrationResponse> Register(ApplicationUser user, string password, UserRole role);

		Task<LoginResponse> Login(LoginRequest loginRequest);
	}
}
