using Microsoft.AspNetCore.Identity;
using SavingsManagementSystem.Data.Contexts;
using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Extensions
{
	public static class IdentityService
	{
		public static void AddIdentityConfig(this IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<SavingsDBContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(opt =>
			{
				opt.User.RequireUniqueEmail = true;
				opt.Password.RequiredLength = 8;
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequireDigit = true;
				opt.Password.RequireUppercase = true;
			});
		}
	}
}
