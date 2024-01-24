using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Common.CustomExceptions
{
	public class SessionExpiredException : Exception
	{
		public SessionExpiredException() : base("User session has expired.")
		{
		}

		public SessionExpiredException(string message) : base(message)
		{
		}

		public SessionExpiredException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

}
