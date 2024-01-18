using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.Transactions.Interfaces
{
	public interface IPaymentService
	{
		Task<InitiatePaymentResponse> InitiatePaymentSessionAsync([FromBody] PaymentRequest model);
		Task PaymentConfirmationAsync(PayConfirmationRequest request);
	}
}
