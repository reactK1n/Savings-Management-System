using Microsoft.EntityFrameworkCore;
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

				// Get the value of the environment variable

				//connectionString = config.GetConnectionString("RenderPostgreConnection");
				var connectionString = Environment.GetEnvironmentVariable("RenderPostgreConnection");
				if (connectionString == null)
				{
					Console.WriteLine("it is not getting any env variable");
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
