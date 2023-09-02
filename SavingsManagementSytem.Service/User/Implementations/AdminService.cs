using Microsoft.AspNetCore.Http;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Service.User.Implementations
{
	public class AdminService : IAdminService
	{

		private readonly IAuthenticationService _auth;
		private readonly IUnitOfWork _unit;

		public AdminService(IAuthenticationService auth, IUnitOfWork unit)
		{
			_unit = unit;
			_auth = auth;
		}

		public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
		{
			var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				EmailConfirmed = true
			};

			var createUser = await _auth.Register(user, request.Password, UserRole.Admin);
			await _unit.SaveChangesAsync();

			var response = new RegistrationResponse
			{
				Id = user.Id,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email
			};

			return response;
		}
	}
}
