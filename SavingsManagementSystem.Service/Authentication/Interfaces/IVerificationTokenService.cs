using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface IVerificationTokenService
	{
		Task<VerificationToken> CreateVerificationTokenAsync(DateTime expiryMinutes, string email, string? status = null, string? userId = null);
	}
}
