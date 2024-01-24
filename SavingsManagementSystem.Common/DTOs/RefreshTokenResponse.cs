namespace SavingsManagementSystem.Common.DTOs
{
	public class RefreshTokenResponse
	{
		public string NewJwtAccessToken { get; set; }
		public string NewRefreshToken { get; set; }
	}
}
