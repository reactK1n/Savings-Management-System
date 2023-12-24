using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
				//var connectionString = config["ConnectionStrings:RenderPostgreConnection"];
				var connectionString = config.GetConnectionString("RenderPostgreConnection");
				Console.WriteLine($"Connection String: {connectionString}");
				if (connectionString == null)
				{
					Console.WriteLine("connection string is null");
					return;
				}
				opt.UseNpgsql(connectionString);

			/*	if (env.IsDevelopment())
				{
					opt.UseSqlServer(config["ConnectionStrings:SqlConnectionString"]);
                    Console.WriteLine("Local db used");
                    return;
				}
                opt.UseInMemoryDatabase("InMemoryDatabase");

				Console.WriteLine("In Memory database used");*/
			});
		}
	}
}
