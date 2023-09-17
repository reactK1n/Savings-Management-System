using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;

namespace SavingsManagementSystem.Service.Authentication.Implementations
{
	public class OTPService : IOTPService
	{
		private readonly IUnitOfWork _unit;

		public OTPService(IUnitOfWork unit)
		{
			_unit = unit;
		}

		public async Task<OTP> CreateOtpAsync(string memberId, int expireMinutes)
		{
			var createdOn = DateTime.UtcNow;
			var otp = new OTP
			{
				Id = Guid.NewGuid().ToString(),
				MemberId = memberId,
				Value = Helper.GenerateOtpValue(),
				IsUsed = false,
				CreatedOn = createdOn,
				Expire = createdOn.AddMinutes(expireMinutes)
			};
			await _unit.OTP.Create(otp);
			return otp;
		}
	}
}
