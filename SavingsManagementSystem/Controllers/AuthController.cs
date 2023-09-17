using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.CustomExceptions;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.Authentication.Interfaces;

namespace SavingsManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthenticationService _authServices;

		public AuthController(IAuthenticationService authServices)
		{
			_authServices = authServices;
		}

		[HttpPost]
		[Route("login")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
		{
			try
			{
				var response = await _authServices.Login(loginRequest);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("forgetPassword")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ForgetPassword([FromQuery] string email)
		{
			try
			{
				var response = await _authServices.ForgetPasswordAsync(email);
				
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("resetPassword")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			try
			{
				var response = await _authServices.ResetPasswordAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("changePassword")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
		{
			try
			{
				var response = await _authServices.ChangePasswordAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[Route("confirmPassword")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> confirmPassword([FromBody] ConfirmEmailRequest request)
		{
			try
			{
				var response = await _authServices.ConfirmEmailAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (LinkExpiredException ex)
			{
				return BadRequest(ex.Message);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
