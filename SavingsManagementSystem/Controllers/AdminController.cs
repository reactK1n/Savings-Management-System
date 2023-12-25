using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IAdminService _adminService;
		private readonly ILogger<AdminController> _logger;
		private readonly IUnitOfWork _unit;
		private readonly UserManager<ApplicationUser> _userManager;



		public AdminController(IAdminService adminService, 
			ILogger<AdminController> logger, 
			IUnitOfWork unit,
			UserManager<ApplicationUser> userManager
)
		{
			_adminService = adminService;
			_logger = logger;
			_unit = unit;
			_userManager = userManager;
		}

		[HttpGet]
		[Route("/")]
		[Route("allUser")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AllUser()
		{
			_logger.LogInformation("admin registration is executing.......");
			try
			{
				var response = "it is working........";
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();

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
		[Route("register")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegistrationRequest request)
		{
			_logger.LogInformation("admin registration is executing.......");
			try
			{
				var response = await _adminService.RegisterAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();

			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpPost]
		[Route("SendInvite")]
		[Authorize(Policy = "Admin")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> SendInvite([FromQuery] string email)
		{
			try
			{
				await _adminService.SendMemberInviteAsync(email);
				return NoContent();
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
	}
}
