using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IUserRepository
	{
		Task Create(ApplicationUser user);

		ICollection<ApplicationUser> FetchUser();

		Task<ApplicationUser> FetchUserAsync(string userId);

		void Update(ApplicationUser user);

		void Delete(ApplicationUser user);
	}
}
