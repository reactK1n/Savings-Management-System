namespace SavingsManagementSystem.Common.CustomExceptions
{
	public class OTPExpiredException : Exception
	{
		public OTPExpiredException() : base("OTP has expired.")
		{
		}

		public OTPExpiredException(string message) : base(message)
		{
		}

		public OTPExpiredException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

}
}
