using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class MemberRepository : GenericRepository<Member>, IMemberRepository
	{
		private readonly DbSet<Member> _dbSet;

		public MemberRepository(SavingsDBContext context) : base(context)
		{
			_dbSet = context.Set<Member>();
		}

		public async Task Create(string userId)
		{
			var member = new Member
			{
				UserId = userId
			};
			await _dbSet.AddAsync(member);
		}

		public ICollection<Member> Fetch()
		{
			return _dbSet.ToList();
		}

		public async Task<Member> FetchByUserIdAsync(string userId)
		{
			var user = await _dbSet.FindAsync(userId);
			return user;
		}

		public async Task<Member> FetchByOtpAsync(string otpId)
		{
			var user = await _dbSet.FindAsync(otpId);
			return user;
		}

		public new void Update(Member member)
		{
			_dbSet.Update(member);
		}

		public new void Delete(Member member)
		{
			_dbSet.Remove(member);
		}
	}
}
