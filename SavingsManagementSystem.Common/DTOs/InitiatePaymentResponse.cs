using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Common.DTOs
{
	public class InitiatePaymentResponse
	{
        public string SessionId { get; set; }

		public string SessionUrl { get; set; }

        public string SessionStatus { get; set; }

        public long? SessionAmount { get; set; }
    }
}
