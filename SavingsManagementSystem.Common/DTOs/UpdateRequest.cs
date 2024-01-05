using Microsoft.AspNetCore.Http;

namespace SavingsManagementSystem.Common.DTOs
{
	public class UpdateRequest
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public string State { get; set; }

		public string City { get; set; }

		public IFormFile Image { get; set; }

	}
}
