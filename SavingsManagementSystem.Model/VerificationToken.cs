using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class VerificationToken
	{
		public string Id { get; set; }

		[ForeignKey(nameof(Member))]
		public string MemberId { get; set; }

		public string Token { get; set; }

		public bool IsUsed { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime ExpiryTime { get; set; }

        //Navigation
        public Member Member { get; set; }

    }
}
