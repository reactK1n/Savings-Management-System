using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Data.Contexts;

namespace SavingsManagementSystem.Extensions
{
    public static class ConnectionConfigurationExtension
	{
		public static void AddDbContextAndConfigurations(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContextPool<SavingsDBContext>(opt =>
			{

					//registering of database service
					opt.UseNpgsql(config["ConnectionStrings:PostgresConnectionString"]);
					

			});
		}
	}
}
