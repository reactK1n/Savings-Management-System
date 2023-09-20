using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface IVerificationTokenService
	{
		Task<VerificationToken> CreateVerificationTokenAsync(int expiryMinutes, string messageStatus, string email, string? userId = null);
	}
}
