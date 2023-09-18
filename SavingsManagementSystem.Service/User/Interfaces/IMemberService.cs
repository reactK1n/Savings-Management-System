using SavingsManagementSystem.Common.DTOs;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Service.User.Interfaces
{
	public interface IMemberService
	{
		Task<RegistrationResponse> RegisterAsync(MemberRegistrationRequest request);
	}
}
