using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.User.Interfaces
{
	public interface IAdminService
	{
		Task<RegistrationResponse> RegisterAsync(AdminRegistrationRequest request);

		Task<string> SendMemberInviteAsync(string email);
	}
}
