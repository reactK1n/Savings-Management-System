using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Repository.Implementations;
using SavingsManagementSystem.Repository.Interfaces;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;

namespace SavingsManagementSystem.Repository.UnitOfWork.Implementations
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly SavingsDBContext _context;
		private IUserRepository _user;
		private IAddressRepository _address;

        public UnitOfWork(SavingsDBContext context)
        {
			_context = context;
        }

		public IUserRepository User
		{
			get => _user ??= new UserRepository(_context);
		}

		public IAddressRepository Address
		{
			get => _address ??= new AddressRepository(_context);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
