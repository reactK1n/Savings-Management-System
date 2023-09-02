using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class SavingRepository : GenericRepository<Saving>, ISavingRepository
	{
		private readonly DbSet<Saving> _dbSet;

		public SavingRepository(SavingsDBContext context) : base(context)
		{
			_dbSet = context.Set<Saving>();
		}

		public async Task Create(Saving saving)
		{
			await _dbSet.AddAsync(saving);
		}

		public ICollection<Saving> Fetch()
		{
			return _dbSet.ToList();
		}

		public async Task<ICollection<Saving>> FetchSavingsAsync(string memberId, DateTime startDate, DateTime endDate)
		{
			var savings = await _dbSet.Where(sa => sa.MemberId == memberId && sa.CreatedOn >= startDate && sa.CreatedOn <= endDate).ToListAsync();
			return savings;
		}

		public async Task<ICollection<Saving>> FetchSavingsAsync(string memberId)
		{
			var savings = await _dbSet.Where(sa => sa.MemberId == memberId).ToListAsync();
			return savings;
		}

		public new void Delete(Saving saving)
		{
			_dbSet.Remove(saving);
		}

	}
}
