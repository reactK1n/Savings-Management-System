using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SavingsManagementSystem.Common.CustomExceptions;
using SavingsManagementSystem.Common.DTOs;
using SavingsManagementSystem.Common.UserRole;
using SavingsManagementSystem.Common.Utilities;
using SavingsManagementSystem.Model;
using SavingsManagementSystem.Repository.UnitOfWork.Interfaces;
using SavingsManagementSystem.Service.Authentication.Interfaces;
using SavingsManagementSystem.Service.Files.Interfaces;
using SavingsManagementSystem.Service.Mail.Interfaces;
using SavingsManagementSystem.Service.User.Interfaces;
using System.Security.Claims;

namespace SavingsManagementSystem.Service.User.Implementations
{
	public class MemberService : IMemberService
	{
		private readonly IAuthenticationService _auth;
		private readonly IUnitOfWork _unit;
		private readonly IVerificationTokenService _vTokenService;
		private readonly IMailService _mailService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IImageService _image;

		public MemberService(
			IAuthenticationService auth,
			IUnitOfWork unit,
			IVerificationTokenService vTokenService,
			IMailService mailService,
			UserManager<ApplicationUser> userManager,
			IHttpContextAccessor httpContextAccessor,
			IImageService image)

		{
			_unit = unit;
			_auth = auth;
			_vTokenService = vTokenService;
			_mailService = mailService;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_image = image;
		}


		public async Task<RegistrationResponse> RegisterAsync(MemberRegistrationRequest request)
		{
			var emailUser = await _userManager.FindByEmailAsync(request.Email);
			if (emailUser != null)
			{
				throw new AlreadyExistsException("email has already registered in our database, Login To your Account ");
			}

			var decodedToken = TokenConverter.DecodeToken(request.Token);
			var vToken = await _unit.VerificationToken.FetchByTokenAsync(decodedToken);
			if (vToken == null || vToken.Email != request.Email)
			{
				throw new ArgumentNullException("Invalid token provided");
			}

			var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				UserName = request.Username,
				EmailConfirmed = true
			};
			var member = new Member
			{
				UserId = user.Id,
				User = user
			};

			var response = await _auth.Register(user, request.Password, UserRole.Member);
			if (response == null)
			{
				throw new ArgumentNullException("Unable to Register User at the moment");
			}
			await _unit.Member.CreateAsync(member);

			//update token IsUsed value
			vToken.IsUsed = true;
			_unit.VerificationToken.Update(vToken);
			await _unit.SaveChangesAsync();

			return response;
		}


		public async Task UpdateUserAsync(UpdateRequest request)
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ArgumentNullException("User not found");
			}
			var address = (await _unit.Address.FetchAsync(user.AddressId)) ?? new Address();

			//assigning values
			user.FirstName = !string.IsNullOrEmpty(request.FirstName) ? request.FirstName : user.FirstName;
			user.LastName = !string.IsNullOrEmpty(request.LastName) ? request.LastName : user.LastName;
			user.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : user.Email;
			user.UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : user.UserName;
			user.PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : user.PhoneNumber;
			user.ImageUri = request.Image != null ? await _image.UploadImageAsync(request.Image) : user.ImageUri;
			address.Street = !string.IsNullOrEmpty(request.Street) ? request.Street : address.Street;
			address.State = !string.IsNullOrEmpty(request.State) ? request.State : address.State;
			address.City = !string.IsNullOrEmpty(request.City) ? request.City : address.City;

			var addressState = _unit.GetEntry(address).State;
			var isAddressAdded = _unit.GetEntry(address).State == EntityState.Added;

			// set the Id property of the address only if it's not set and the entity is being added
			if (string.IsNullOrEmpty(address.Id) && !isAddressAdded)
			{
				user.AddressId = address.Id = Guid.NewGuid().ToString();
				address.CreatedOn = DateTime.UtcNow;
				addressState = EntityState.Added;
			}

			//updating or adding entities
			if (addressState == EntityState.Added)
			{
				await _unit.Address.CreateAsync(address);
			}
			else
			{
				_unit.Address.Update(address);
			}
			_unit.User.Update(user);

			//saving changes
			await _unit.SaveChangesAsync();
		}

	}
}
