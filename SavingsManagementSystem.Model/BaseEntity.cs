using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsManagementSystem.Model
{
	public class BaseEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? UpdatedOn { get; set; }
	}
}
