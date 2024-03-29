﻿using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.User.Interfaces
{
	public interface IMemberService
	{
		Task<RegistrationResponse> RegisterAsync(MemberRegistrationRequest request);

		Task UpdateUserAsync(UpdateRequest request);
	}
}
