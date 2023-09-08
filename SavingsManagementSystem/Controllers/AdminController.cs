using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IAdminService _adminService;

		public AdminController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		[HttpPost]
		[Route("register")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
		{
			try
			{
				var response = await _adminService.RegisterAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();

			}
			catch (MissingFieldException ex)
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
	}
}
