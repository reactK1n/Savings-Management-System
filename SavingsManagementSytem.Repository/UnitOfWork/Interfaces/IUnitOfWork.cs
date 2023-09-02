using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.UnitOfWork.Interfaces
{
	public interface IUnitOfWork
	{
		IUserRepository User { get; }

		IAddressRepository Address { get; }

		Task SaveChangesAsync();
	}
}
