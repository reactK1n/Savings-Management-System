using Microsoft.AspNetCore.Authorization;

namespace SavingsManagementSystem.Policies
{
	public class Policies
	{
		/// <summary>
		/// Policy for Admin role
		/// </summary>
		public const string Admin = "Admin";
		/// <summary>
		/// Policy for a Regular User role
		/// </summary>
		public const string Member = "Member";
		/// <summary>
		/// Policy for an Admin and a particular member
		/// </summary>
		public const string AdminAndMember = "AdminAndMember";
		/// <summary>
		/// Grants Admin User Rights
		/// </summary>
		/// <returns></returns>
		public static AuthorizationPolicy AdminPolicy()
		{
			return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
		}

		/// <summary>
		/// Grants Regular User Users Rights
		/// </summary>
		/// <returns></returns>
		public static AuthorizationPolicy MemberPolicy()
		{
			return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Member).Build();
		}

		/// <summary>
		/// Grants Admin and Member User Rights
		/// </summary>
		/// <returns></returns>
		public static AuthorizationPolicy AdminAndMemberPolicy()
		{
			return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin, Member).Build();
		}
	}
}
