using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Data
{
	public class SavingsDBContext : IdentityDbContext<ApplicationUser>
	{
		public SavingsDBContext(DbContextOptions<SavingsDBContext> option) : base(option) { }

		public DbSet<Address> Addresses { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Saving> Savings { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<OTP> OTPs { get; set; }

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var item in ChangeTracker.Entries<BaseEntity>())
			{
				switch (item.State)
				{
					case EntityState.Modified:
						item.Entity.UpdatedOn = DateTime.UtcNow;
						break;
					case EntityState.Added:
						item.Entity.Id = item.Entity.Id ?? Guid.NewGuid().ToString();
						item.Entity.CreatedOn = DateTime.UtcNow;
						break;
					default:
						break;
				}
			}
			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}
