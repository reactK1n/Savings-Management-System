using Microsoft.AspNetCore.Mvc;
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
	}
}
