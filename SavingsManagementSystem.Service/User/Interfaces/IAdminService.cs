using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.User.Interfaces
{
	public interface IAdminService
	{
		Task<RegistrationResponse> RegisterAsync(AdminRegistrationRequest request);

		Task SendMemberInviteAsync(string email);

		Task UpdateUserAsync(UpdateRequest request);

		Task DeleteUserAsync(string userId);
	}
}
