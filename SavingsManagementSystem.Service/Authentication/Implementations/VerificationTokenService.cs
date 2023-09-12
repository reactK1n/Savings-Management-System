using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;

namespace SavingsManagementSystem.Service.Authentication.Implementations
{
	public class VerificationTokenService : IVerificationTokenService
	{
		private readonly IUnitOfWork _unit;

		public VerificationTokenService(IUnitOfWork unit)
		{
			_unit = unit;
		}

		public async Task<VerificationToken> CreateVerificationTokenAsync(string userId, int expiryMinutes)
		{
			var createdOn = DateTime.UtcNow;
			var vToken = new VerificationToken
			{
				Id = Guid.NewGuid().ToString(),
				UserId = userId,
				Token = Guid.NewGuid().ToString(),
				IsUsed = false,
				CreatedOn = createdOn,
				ExpiryTime = createdOn.AddMinutes(expiryMinutes)
			};
			await _unit.VerificationToken.Create(vToken);
			return vToken;
		}
	}
}
