using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SavingsManagementSystem.Data.Contexts;

namespace SavingsManagementSystem.Extensions
{
	public static class ConnectionConfigurationExtension
	{
		public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
		{

			services.AddDbContextPool<SavingsDBContext>(opt =>
			{

				//registering of database service
				//opt.UseNpgsql(config.GetConnectionString("PostgresConnectionString"));
				//opt.UseNpgsql(config["ConnectionStrings:PostgresConnectionString"]);
				if (env.IsDevelopment())
				{
					opt.UseSqlServer(config["ConnectionStrings:SqlConnectionString"]);
                    Console.WriteLine("Local db used");
                    return;
				}
                opt.UseInMemoryDatabase("InMemoryDatabase");

				Console.WriteLine("In Memory database used");
			});
		}
	}
}
