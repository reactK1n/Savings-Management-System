using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.User.Interfaces
{
	public interface IAdminService
	{
		Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
	}
}
