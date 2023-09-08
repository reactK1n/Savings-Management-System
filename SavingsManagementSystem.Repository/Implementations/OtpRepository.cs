using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;
using static System.Net.WebRequestMethods;

namespace SavingsManagementSystem.Repository.Implementations
{
	public class OtpRepository : GenericRepository<OTP>, IOtpRepository
	{
		private readonly DbSet<OTP> _dbSet;

		public OtpRepository(SavingsDBContext context) : base(context)
		{
			_dbSet = context.Set<OTP>();
		}

		public async Task Create(OTP otp)
		{
			await _dbSet.AddAsync(otp);
		}

		public ICollection<OTP> Fetch()
		{
			return _dbSet.ToList();
		}

		public async Task<OTP> FetchAsync(string otpId)
		{
			var otp = await _dbSet.FindAsync(otpId);
			return otp;
		}

		public new void Delete(OTP otp)
		{
			_dbSet.Remove(otp);
		}
	}
}
