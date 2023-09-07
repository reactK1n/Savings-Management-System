using FluentValidation;
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

namespace SavingsManagementSystem.Extensions
{
	public static class DIServiceExtension
	{
		public static void AddDependencyInjection(this IServiceCollection services)
		{
			//add httpContext accessor
			services.AddHttpContextAccessor();
			//Sevices DI
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<ITokenService, TokenService>();

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
