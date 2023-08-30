using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class Member : BaseEntity
	{
		[ForeignKey(nameof(User))]
		public string UserId { get; set; }

		//navigation propertie
		public ApplicationUser User { get; set; }

		public ICollection<OTP> OTP { get; set; }

		public ICollection<Saving> Savings { get; set; }


	}
}
