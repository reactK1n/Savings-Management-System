namespace SavingsManagementSystem.Model
{
	public class BaseEntity
	{
		public string Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? UpdatedOn { get; set; }
	}
}
