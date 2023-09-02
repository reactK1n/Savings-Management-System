using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Common.DTOs
{
	public class LoginResponse
	{
		public string Id { get; set; }	

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Token { get; set; }

	}
}
