using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface ISavingRepository
	{
		Task Create(Saving saving);

		ICollection<Saving> Fetch();

		Task<ICollection<Saving>> FetchSavingsAsync(string memberId, DateTime startDate, DateTime endDate);

		Task<ICollection<Saving>> FetchSavingsAsync(string memberId);

		void Delete(Saving saving);
	}
}
