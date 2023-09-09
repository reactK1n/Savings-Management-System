using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;

namespace SavingsManagementSystem.Service.OTP.Implementations
{
	public class OtpService
	{
		private readonly IUnitOfWork _unit;

		public OtpService(IUnitOfWork unit)
        {
			_unit = unit;
		}

	}
}
