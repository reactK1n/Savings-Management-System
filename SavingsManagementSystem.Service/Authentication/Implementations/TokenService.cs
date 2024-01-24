using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SavingsManagementSystem.Service.Authentication.Implementations
{
	public class TokenService : ITokenService 
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _config;

		public TokenService(UserManager<ApplicationUser> userManager, IConfiguration config)
		{
			_userManager = userManager;
			_config = config;
		}
		public async Task<string> GetToken(ApplicationUser user)
		{

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Name, $"{user.LastName} {user.FirstName}")
			};

			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}
			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));

			var getToken = new JwtSecurityToken(
				audience: _config["JwtSettings:ValidAudience"],
				issuer: _config["JwtSettings:ValidIssuer"],
				claims: authClaims,
				expires: DateTime.UtcNow.AddDays(2),
				signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
				);

			return new JwtSecurityTokenHandler().WriteToken(getToken);
		}

		public string GenerateRefreshToken()
		{
			// Generate a custom token with Guid
			string refreshToken = Guid.NewGuid().ToString();

			// Return the custom token
			return refreshToken;
		}

	}
}
