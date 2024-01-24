using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class AddressRepository : GenericRepository<Address>, IAddressRepository
	{
        private readonly DbSet<Address> _dbSet;
		private readonly SavingsDBContext _context;
		public AddressRepository(SavingsDBContext context) : base(context)
        {
			_context = context;
            _dbSet = context.Set<Address>();
        }

		public new async Task CreateAsync(Address address)
		{
			await _dbSet.AddAsync(address);
		}

		public async Task<Address> FetchAsync(string addressId)
		{
			var address = await _dbSet.FindAsync(addressId);
			return address;
		}

		public new void Update(Address address)
		{
			_dbSet.Update(address);
		}

		public new void Delete(Address address)
		{
			_dbSet.Remove(address);
		}

	}
}
