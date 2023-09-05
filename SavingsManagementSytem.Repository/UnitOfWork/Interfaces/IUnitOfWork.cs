using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.UnitOfWork.Interfaces
{
	public interface IUnitOfWork
	{
		IUserRepository User { get; }
		IAddressRepository Address { get; }

		IMemberRepository Member { get; }

		ISavingRepository Saving { get; }

		IOtpRepository OTP { get; }

		ITransactionRepository Transaction { get; }

		Task SaveChangesAsync();
	}
}
