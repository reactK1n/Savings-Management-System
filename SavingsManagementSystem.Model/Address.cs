﻿namespace SavingsManagementSystem.Model
{
	public class Address : BaseEntity
	{
		public string? Street { get; set; }

		public string? City { get; set; }

		public string? State { get; set; }

		//Navigation
		public ApplicationUser? User { get; set; }
	}
}
