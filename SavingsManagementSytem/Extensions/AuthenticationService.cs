﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SavingsManagementSystem.Extensions
{
	public static class AuthenticationService
	{
		public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters

				{
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidAudience = configuration["JwtSettings:ValidAudience"],
					ValidIssuer = configuration["JwtSettings:ValidIssuer"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
					ClockSkew = TimeSpan.Zero
				};
			});
		}
	}
}
