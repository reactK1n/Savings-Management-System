namespace SavingsManagementSystem.Common.DTOs
{
	public class ResetPasswordRequest
	{
		public string Password { get; set; }

		public string VToken { get; set; }

		public string UserId { get; set; }

	}
}
