using Microsoft.OpenApi.Models;

namespace SavingsManagementSystem.Extensions
{
	public static class SwaggerExtension
	{
		public static void AddSwaggerConfig(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SavingsManagementSystem", Version = "v1" });
				//this for locking the api with roles inside controller
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter 'Bearer' [space] and then your valid token in the input below.\r\n\rExample: \"Bearer ioqnqf8uqnwifqiwunwfudifijdfdlkjsdnfajldjnfj"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{

						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});
		}
	}
}
