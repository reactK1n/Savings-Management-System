namespace SavingsManagementSystem.Common.Utilities
{
	public static class Helper
	{
		public static string GenerateOtpValue()
		{
			return $"{new Random().Next(0, 9)}{Guid.NewGuid}{new Random().Next(0, 9)}";
		}
	}
}
