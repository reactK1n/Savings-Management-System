using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class VerificationToken
	{
		public string Id { get; set; }

		[ForeignKey(nameof(ApplicationUser))]
		public string? UserId { get; set; }

		public string Token { get; set; }

		public bool IsUsed { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime ExpiryTime { get; set; }

        //Navigation
        public ApplicationUser User { get; set; }

    }
}
