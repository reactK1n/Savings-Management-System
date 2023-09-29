namespace SavingsManagementSystem.Common.CustomExceptions
{
	public class AlreadyExistsException : Exception
	{
		public AlreadyExistsException() : base("Already exists in our Database")
		{
		}

		public AlreadyExistsException(string message) : base(message)
		{
		}

		public AlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

