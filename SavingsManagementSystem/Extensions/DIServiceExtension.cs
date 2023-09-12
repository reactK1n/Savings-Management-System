﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.Validators.AuthenticationValidator;
using SavingsManagementSystem.Repository.Implementations;
using SavingsManagementSystem.Repository.Interfaces;
using SavingsManagementSystem.Repository.UnitOfWork.Implementations;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Implementations;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.Mail.Implementations;
using SavingsManagementSystem.Service.Mail.Interfaces;
using SavingsManagementSystem.Service.User.Implementations;
using SavingsManagementSystem.Service.User.Interfaces;
using Microsoft.AspNetCore.Mvc.Routing;
using SavingsManagementSystem.Common.Utilities;

namespace SavingsManagementSystem.Extensions
{
	public static class DIServiceExtension
	{
		public static void AddDependencyInjection(this IServiceCollection services)
		{
			//add IhttpContext accessor
			services.AddHttpContextAccessor();

			// Register your GenerateLink class
			services.AddScoped<GenerateLink>();

			// Add the required services for LinkGenerator
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddScoped<IUrlHelper>(x => new UrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

			//Sevices DI
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IOTPService, OTPService>();


			//repository DI
			services.AddScoped<IMemberRepository, MemberRepository>();
			services.AddScoped<ISavingRepository, SavingRepository>();
			services.AddScoped<IOtpRepository, OtpRepository>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IMailService, MailService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			//registering Fluent validations injection class
			services.AddScoped<IValidator<LoginRequest>, LoginRequestValidation>();
			services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordRequestValidation>();
			services.AddScoped<IValidator<RegistrationRequest>, RegistrationRequestValidation>();
			services.AddScoped<IValidator<MailRequest>, MailRequestValidator>();
			services.AddScoped<IValidator<ConfirmEmailRequest>, ConfirmEmailRequestValidator>();
			services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();

		}
	}
}