using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IMemberRepository
	{
		Task Create(Member user);

		ICollection<Member> Fetch();

		Task<Member> FetchUserAsync(string userId);

		void Update(Member user);

		void Delete(Member user);
	}
}
