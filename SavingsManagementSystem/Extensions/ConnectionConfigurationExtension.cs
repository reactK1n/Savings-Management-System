using Microsoft.EntityFrameworkCore;
using Npgsql;
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
				//string connectionString = Environment.GetEnvironmentVariable("RenderPostgreConnection");
				var builder = new NpgsqlConnectionStringBuilder
				{
					Host = Environment.GetEnvironmentVariable("Host"),
					Port = Convert.ToInt32(Environment.GetEnvironmentVariable("Port")),
					Database = Environment.GetEnvironmentVariable("Database"),
					Username = Environment.GetEnvironmentVariable("Username"),
					Password = Environment.GetEnvironmentVariable("Password")
				};

				string connectionString = builder.ToString();

				if (connectionString == null)
				{
					Console.WriteLine("it is not getting any env variable");
					return;
				}

				/*var connectionString = config.GetConnectionString("RenderPostgreConnection");
				if (connectionString == null)
				{
					Console.WriteLine("connection string is null");
					return;
			}*/
				Console.WriteLine(connectionString);

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
