namespace SavingsManagementSystem.Common.DTOs
{
	public class PaymentRequest
	{
		public decimal Amount { get; set; }
		public string ProductName { get; set; }
		public string ProductDescription { get; set; }
	}
}
