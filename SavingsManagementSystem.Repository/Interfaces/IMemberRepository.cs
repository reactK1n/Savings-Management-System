using SavingsManagementSystem.Model;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IMemberRepository
	{
		Task Create(Member user);

		ICollection<Member> Fetch();

		Task<Member> FetchByUserIdAsync(string userId);

		Task<Member> FetchByOtpAsync(string otpId);

		void Update(Member user);

		void Delete(Member user);
	}
}
