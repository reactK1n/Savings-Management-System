using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IAddressRepository
	{
		Task CreateAsync(Address address);

		Task<Address> FetchAsync(string addressId);

		void Update(Address address);

		void Delete(Address address);
	}
}
