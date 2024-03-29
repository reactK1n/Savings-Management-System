﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.Transactions.Interfaces;
using Stripe;

namespace SavingsManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}



		[HttpPost]
		[Authorize(Policy = "Member")]
		[Route("Initiate-Payment")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest request)
		{
			try
			{
				var response = await _paymentService.InitiatePaymentSessionAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();

			}

			catch (StripeException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}



		[HttpPost]
		[Authorize(Policy = "Member")]
		[Route("Payment-Confirmation")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> PaymentConfirmation([FromBody] PayConfirmationRequest request)
		{
			try
			{
				await _paymentService.PaymentConfirmationAsync(request);
				return Ok("Payment Completed");
			}

			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (StripeException ex)
			{
				return BadRequest(ex.Message);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[HttpGet]
		[Authorize(Policy = "Admin")]
		[Route("Get_All_Members_Payment")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllPayment()
		{
			try
			{
				var response = await _paymentService.GetAllPaymentsAsync();
				return Ok(response);
			}

			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpGet]
		[Authorize(Policy = "Member")]
		[Route("Get_Member_Payment")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetPayments()
		{
			try
			{
				var response = await _paymentService.GetPaymentsWithAuthorizedMenberAsync();
				return Ok(response);
			}

			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[HttpGet]
		[Authorize(Policy = "Admin")]
		[Route("Get_Members_Total_Payment")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetMembersTotalPayment()
		{
			try
			{
				var response = await _paymentService.GetTotalPayAsync();
				return Ok(response);
			}

			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[HttpGet]
		[Authorize(Policy = "Member")]
		[Route("Get_Member_Total_Payment")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetMemberTotalPayment(string memberId)
		{
			try
			{
				var response = await _paymentService.GetTotalPayWithIdAsync(memberId);
				return Ok(response);
			}

			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
