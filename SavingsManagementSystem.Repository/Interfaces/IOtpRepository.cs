using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IOtpRepository
	{
		Task Create(OTP otp);

		ICollection<OTP> Fetch();

		Task<OTP> FetchByOtpIdAsync(string otpId);

		Task<OTP> FetchByValueAsync(string value);

		Task<ICollection<OTP>> FetchAllAsync(string userId);

		void Update(OTP otp);

		void Delete(OTP otp);
	}
}
