using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SavingsManagementSystem.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SavingsManagementSystem.Service.Authentication.Interfaces
{
	public interface ITokenService
	{
		Task<string> GetToken(ApplicationUser user);

	}
}
