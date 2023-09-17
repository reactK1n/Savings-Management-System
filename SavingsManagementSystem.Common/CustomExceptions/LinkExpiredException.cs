namespace SavingsManagementSystem.Common.CustomExceptions
{
	public class LinkExpiredException : Exception
	{
		public LinkExpiredException() : base("Link has expired.")
		{
		}

		public LinkExpiredException(string message) : base(message)
		{
		}

		public LinkExpiredException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

}

