using System.ComponentModel.DataAnnotations;

namespace SavingsManagementSystem.Common.DTOs
{
	public class RegistrationRequest
	{
		[Required]
		public string FirstName { get; set; }
	
		[Required]
		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
