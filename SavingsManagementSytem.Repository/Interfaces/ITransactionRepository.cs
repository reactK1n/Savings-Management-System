using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface ITransactionRepository
	{
		Task Create(Transaction transaction);

		ICollection<Transaction> Fetch();

		Task<ICollection<Transaction>> FetchTransactionAsync(string memberId, DateTime startDate, DateTime endDate);

		Task<ICollection<Transaction>> FetchTransactionsAsync(string memberId);

		void Delete(Transaction transaction);
	}
}
