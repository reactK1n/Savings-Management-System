using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class VTRepository : GenericRepository<VerificationToken>, IVTRepository
	{
		private readonly DbSet<VerificationToken> _dbSet;

		public VTRepository(SavingsDBContext context) : base(context)
		{
			_dbSet = context.Set<VerificationToken>();
		}

		public async Task Create(VerificationToken vToken)
		{
			await _dbSet.AddAsync(vToken);
		}

		public ICollection<VerificationToken> Fetch()
		{
			return _dbSet.ToList();
		}

		public async Task<VerificationToken> FetchByvTokenIdAsync(string vTokenId)
		{
			var vToken = await _dbSet.FindAsync(vTokenId);
			return vToken;
		}

		public async Task<VerificationToken> FetchByTokenAsync(string token)
		{
			var vToken = await _dbSet.FirstOrDefaultAsync(o => o.Token == token); 
			return vToken;
		}

		public async Task<ICollection<VerificationToken>> FetchAllAsync(string userId)
		{
			var vTokens = await _dbSet.Where(o => o.UserId == userId).ToListAsync();
			return vTokens;
		}

		public new void Update(VerificationToken vToken)
		{
			_dbSet.Update(vToken);
		}

		public new void Delete(VerificationToken vToken)
		{
			_dbSet.Remove(vToken);
		}
	}
}
