using System.ComponentModel.DataAnnotations;

namespace SavingsManagementSystem.Common.DTOs
{
	public class RegistrationRequest
	{
		[Required]
		public string FirstName { get; set; }
	
		[Required]
		public string LastName { get; set; }

		public string Email { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

		public string Password { get; set; }
	}
}
