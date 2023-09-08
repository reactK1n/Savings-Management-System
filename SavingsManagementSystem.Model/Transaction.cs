namespace SavingsManagementSystem.Model
{
	public class Transaction : BaseEntity
	{
        public string MemberId { get; set; }

		public string TransactionType { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public string Reference { get; set; }

        //nav property
        public Member Member { get; set; }
    }
}
