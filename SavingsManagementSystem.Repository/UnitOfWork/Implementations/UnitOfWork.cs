using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
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
		private IMemberRepository _member;
		private IOtpRepository _otp;
		private ISavingRepository _saving;
		private ITransactionRepository _transaction;
		private IVTRepository _verificationToken;

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

		public IMemberRepository Member
		{
			get => _member ??= new MemberRepository(_context);
		}

		public ISavingRepository Saving
		{
			get => _saving ??= new SavingRepository(_context);
		}

		public IOtpRepository OTP
		{
			get => _otp ??= new OtpRepository(_context);
		}

		public IVTRepository VerificationToken
		{
			get => _verificationToken ??= new VTRepository(_context);
		}

		public ITransactionRepository Transaction
		{
			get => _transaction ??= new TransactionRepository(_context);
		}

		public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class
		{
			return _context.Entry(entity);
		}

		public EntityEntry<TEntity> AttachEntity<TEntity>(TEntity entity) where TEntity : class
		{
			return _context.Attach(entity);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
