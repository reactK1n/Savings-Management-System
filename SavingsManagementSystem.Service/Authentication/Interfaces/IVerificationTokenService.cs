using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface IVerificationTokenService
	{
		Task<VerificationToken> CreateVerificationTokenAsync(string userId, int expiryMinutes);
	}
}
