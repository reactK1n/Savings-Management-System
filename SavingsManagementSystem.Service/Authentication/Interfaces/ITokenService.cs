using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface ITokenService
	{
		Task<string> GetToken(ApplicationUser user);

		string GenerateRefreshToken();
	}
}
