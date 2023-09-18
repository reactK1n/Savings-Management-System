using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IMemberRepository
	{
		Task Create(string userId);

		ICollection<Member> Fetch();

		Task<Member> FetchByUserIdAsync(string userId);

		Task<Member> FetchByOtpAsync(string otpId);

		void Update(Member member);

		void Delete(Member member);
	}
}
