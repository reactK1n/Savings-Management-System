namespace SavingsManagementSystem.Policies
{
	public static class AuthorizationPolicyService
	{
		public static void AddPolicyAuthorization(this IServiceCollection services)
		{
			services.AddAuthorization(configure =>
			{
				configure.AddPolicy(Policies.Admin, Policies.AdminPolicy());
				configure.AddPolicy(Policies.Member, Policies.MemberPolicy());
				configure.AddPolicy(Policies.AdminAndMember, Policies.AdminAndMemberPolicy());
			});
		}
	}
}
