using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.Interfaces;

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

		public async Task<OTP> FetchByOtpIdAsync(string otpId)
		{
			var otp = await _dbSet.FindAsync(otpId);
			return otp;
		}

		public async Task<OTP> FetchByValueAsync(string value)
		{
			var otp = await _dbSet.FindAsync(value);
			return otp;
		}

		public async Task<ICollection<OTP>> FetchAllAsync(string userId)
		{
			var otps = await _dbSet.Where(o => o.UserId == userId).ToListAsync();
			return otps;
		}

		public new void Update(OTP otp)
		{
			_dbSet.Update(otp);
		}

		public new void Delete(OTP otp)
		{
			_dbSet.Remove(otp);
		}
	}
}
