using Serilog;

namespace SavingsManagementSystem.Extensions
{
	public static class LoggingServiceExtension
	{
		public static void AddLogger(this WebApplicationBuilder builder )
		{
			var logger = new LoggerConfiguration()
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext()
			.CreateLogger();
			builder.Logging.ClearProviders();
			builder.Logging.AddSerilog(logger);
		}
	
	}
}
