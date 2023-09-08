using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IOtpRepository
	{
		Task Create(OTP otp);

		ICollection<OTP> Fetch();

		Task<OTP> FetchAsync(string otpId);

		void Delete(OTP otp);
	}
}
