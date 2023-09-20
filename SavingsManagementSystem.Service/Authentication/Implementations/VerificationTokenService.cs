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

		public async Task<VerificationToken> CreateVerificationTokenAsync(int expiryMinutes, string status, string email, string? userId = null)
		{
			var createdOn = DateTime.UtcNow;
			var vToken = new VerificationToken
			{
				Id = Guid.NewGuid().ToString(),
				UserId = userId,
				Token = Guid.NewGuid().ToString(),
				Status = status,
				Email = email,
				IsUsed = false,
				CreatedOn = createdOn,
				ExpiryTime = createdOn.AddMinutes(expiryMinutes)
			};
			await _unit.VerificationToken.Create(vToken);
			return vToken;
		}
	}
}
