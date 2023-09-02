using SavingsManagementSystem.Repository.Implementations;
using SavingsManagementSystem.Repository.Interfaces;
using SavingsManagementSystem.Repository.UnitOfWork.Implementations;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Implementations;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.User.Implementations;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Extensions
{
	public static class DIServiceExtension
	{
		public static void AddDependencyInjection(this IServiceCollection services)
		{
			//Sevices DI
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<ITokenService, TokenService>();

			//repository DI
			services.AddScoped<IMemberRepository, MemberRepository>();
			services.AddScoped<ISavingRepository, SavingRepository>();
			services.AddScoped<IOtpRepository, OtpRepository>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}
	}
}
