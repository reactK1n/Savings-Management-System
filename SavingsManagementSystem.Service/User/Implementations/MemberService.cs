using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using SavingsManagementSystem.Common.CustomExceptions;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.Mail.Interfaces;
using SavingsManagementSystem.Service.User.Interfaces;

namespace SavingsManagementSystem.Service.User.Implementations
{
	public class MemberService : IMemberService
	{
		private readonly IAuthenticationService _auth;
		private readonly IUnitOfWork _unit;
		private readonly IVerificationTokenService _vTokenService;
		private readonly IMailService _mailService;
		private readonly UserManager<ApplicationUser> _userManager;

		public MemberService(IAuthenticationService auth,
			IUnitOfWork unit,
			IVerificationTokenService vTokenService,
			IMailService mailService,
			UserManager<ApplicationUser> userManager)
		{
			_unit = unit;
			_auth = auth;
			_vTokenService = vTokenService;
			_mailService = mailService;
			_userManager = userManager;
		}

		public async Task<RegistrationResponse> RegisterAsync(MemberRegistrationRequest request)
		{
			var emailUser = _userManager.FindByEmailAsync(request.Email);
            if (emailUser.Result != null)
            {
				throw new AlreadyExistsException("email has already registered in our database, Login To your Account ");
            }
            var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				UserName = request.Username,
				EmailConfirmed = true
			};

			var response = await _auth.Register(user, request.Password, UserRole.Member);
			if (response == null)
			{
				throw new ArgumentNullException("No response provided");
			}
			var decodedToken = TokenConverter.DecodeToken(request.Token);
			var vToken = await _unit.VerificationToken.FetchByTokenAsync(decodedToken);
            if (vToken == null)
            {
				throw new ArgumentNullException("Invalid token provided");
            }

			await _unit.Member.Create(user.Id);
			//update token IsUsed value
            vToken.IsUsed = true;
			_unit.VerificationToken.Update(vToken);
			await _unit.SaveChangesAsync();

			return response;
		}
	}
}
