using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class VerificationToken : BaseEntity
	{
		[ForeignKey(nameof(ApplicationUser))]
		public string? UserId { get; set; }

		public string? Token { get; set; }

		public bool IsUsed { get; set; }

		public string? Email { get; set; }

		public string? Status { get; set; }

		public DateTime? ExpiryTime { get; set; }

        //Navigation
        public ApplicationUser User { get; set; }

    }
}
