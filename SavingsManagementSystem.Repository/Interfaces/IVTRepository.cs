using SavingsManagementSystem.Model;

namespace SavingsManagementSystem.Repository.Interfaces
{
	public interface IVTRepository
	{
		Task Create(VerificationToken vToken);

		ICollection<VerificationToken> Fetch();

		Task<VerificationToken> FetchByvTokenIdAsync(string vTokenId);

		Task<VerificationToken> FetchByTokenAsync(string token);

		Task<ICollection<VerificationToken>> FetchAllAsync(string userId);

		void Update(VerificationToken vToken);

		void Delete(VerificationToken vToken);
	}
}
