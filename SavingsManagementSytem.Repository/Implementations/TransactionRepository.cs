using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
	{
		private readonly DbSet<Transaction> _dbSet;

		public TransactionRepository(SavingsDBContext context) : base(context)
		{
			_dbSet = context.Set<Transaction>();
		}

		public async Task Create(Transaction transaction)
		{
			await _dbSet.AddAsync(transaction);
		}

		public ICollection<Transaction> Fetch()
		{
			return _dbSet.ToList();
		}

		public async Task<ICollection<Transaction>> FetchTransactionAsync(string memberId, DateTime startDate, DateTime endDate)
		{
			var transactions = await _dbSet.Where(tr => tr.MemberId == memberId && tr.CreatedOn >= startDate && tr.CreatedOn <= endDate ).ToListAsync();
			return transactions;
		}

		public async Task<ICollection<Transaction>> FetchTransactionsAsync(string memberId)
		{
			var transactions = await _dbSet.Where(tr => tr.MemberId == memberId).ToListAsync();
			return transactions;
		}

		public new void Delete(Transaction transaction)
		{
			_dbSet.Remove(transaction);
		}
	}
}
