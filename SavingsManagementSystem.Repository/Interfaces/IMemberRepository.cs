using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IMemberRepository
	{
		Task CreateAsync(Member member);

		ICollection<Member> Fetch();

		Task<Member> FetchByUserIdAsync(string userId);

		Task<Member> FetchByMemberIdAsync(string memberId);

		Task<Member> FetchByOtpAsync(string otpId);

		void Update(Member member);

		void Delete(Member member);
	}
}
