using Microsoft.AspNetCore.Mvc;
using SavingsManagementSystem.Common.CustomExceptions;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MemberController : Controller
	{
		private readonly IMemberService _memberService;

		public MemberController(IMemberService memberService)
		{
			_memberService = memberService;
		}

		[HttpPost]
		[Route("register")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> RegisterMember([FromBody] MemberRegistrationRequest request)
		{
			try
			{
				var response = await _memberService.RegisterAsync(request);
				if (response != null)
				{
					return Ok(response);
				}
				return BadRequest();

			}
			catch(AlreadyExistsException ex)
			{
				return BadRequest(ex.Message);
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

