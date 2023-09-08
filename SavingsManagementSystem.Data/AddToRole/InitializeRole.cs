using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SavingsManagementSystem.Common.UserRole;

namespace SavingsManagementSystem.Data.AddToRole
{
	public static class InitializeRole
	{
		public static void InitRoles(this IApplicationBuilder app)
		{
			var serviceScope = app.ApplicationServices.CreateScope();
			var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var roles = roleManager.Roles.ToList();
			if (!roles.Any())
			{
				roleManager.CreateAsync(new IdentityRole { Name = UserRole.Admin.ToString() }).Wait();
				roleManager.CreateAsync(new IdentityRole { Name = UserRole.Member.ToString() }).Wait();
			}
		}
	}
}
