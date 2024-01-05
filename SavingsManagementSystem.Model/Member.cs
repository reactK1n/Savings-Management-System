using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class Member : BaseEntity
	{
		[ForeignKey(nameof(ApplicationUser))]
		public string UserId { get; set; }


		//navigation properties
		public ApplicationUser User { get; set; }

		public ICollection<Saving> Savings { get; set; }

		public ICollection<OTP> OTPs { get; set; }

		public ICollection<Transaction> Transactions { get; set; }

	}
}
