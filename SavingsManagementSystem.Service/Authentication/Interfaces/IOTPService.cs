using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface IOTPService
	{
		Task<OTP> CreateOtpAsync(string memberId, int expireMinutes);
	}
}
