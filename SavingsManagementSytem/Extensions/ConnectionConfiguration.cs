using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data;

namespace SavingsManagementSystem.Extensions
{
	public static class ConnectionConfiguration
	{
		public static void AddDbContextAndConfigurations(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContextPool<SavingsDBContext>(opt =>
			{
				//registering of database service
				opt.UseSqlServer(config["ConnectionStrings:DefaultConnectionString"]);
			});
		}
	}
}
