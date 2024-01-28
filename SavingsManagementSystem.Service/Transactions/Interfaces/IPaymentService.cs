using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;

namespace SavingsManagementSystem.Service.Transactions.Interfaces
{
	public interface IPaymentService
	{
		Task<InitiatePaymentResponse> InitiatePaymentSessionAsync([FromBody] PaymentRequest model);
		Task PaymentConfirmationAsync(PayConfirmationRequest request);

		Task<ICollection<TransactionResponse>> GetAllPaymentsAsync();

		Task<string> GetTotalPayAsync();

		Task<string> GetTotalPayWithIdAsync(string memberId);

		Task<ICollection<TransactionResponse>> GetPaymentsWithAuthorizedMenberAsync();
	}
}
