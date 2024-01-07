using Microsoft.EntityFrameworkCore.ChangeTracking;
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

		IVTRepository VerificationToken { get; }

		ITransactionRepository Transaction { get; }

		EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class;

		EntityEntry<TEntity> AttachEntity<TEntity>(TEntity entity) where TEntity : class;

		Task SaveChangesAsync();
	}
}
