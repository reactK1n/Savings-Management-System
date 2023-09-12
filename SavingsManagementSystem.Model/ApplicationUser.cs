using Microsoft.AspNetCore.Identity;

namespace SavingsManagementSystem.Model
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string? ImageUri { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime? RefreshTokenExpiryTime { get; set; }

		public string? AddressId { get; set; }
		//nav property

		public Address Address { get; set; }
	}
}
