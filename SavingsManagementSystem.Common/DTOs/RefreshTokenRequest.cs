using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Common.DTOs
{
	public class RefreshTokenRequest
	{
			public string UserId { get; set; }
			public Guid RefreshToken { get; set; }
	}
}
