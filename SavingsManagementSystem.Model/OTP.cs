using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class OTP
	{
		public string Id { get; set; }

		[ForeignKey(nameof(ApplicationUser))]
		public string UserId { get; set; }

		public string Value { get; set; }

		public bool IsUsed { get; set; }

		public DateTime Expire { get; set; }

		public DateTime CreatedOn { get; set; }


		//navigation property
		public ApplicationUser User { get; set; }

	}
}
