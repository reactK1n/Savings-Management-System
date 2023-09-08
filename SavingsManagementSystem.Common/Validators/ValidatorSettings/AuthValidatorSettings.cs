using FluentValidation;

namespace SavingsManagementSystem.Common.Validators.ValidatorSettings
{
	public static class AuthValidatorSettings
	{
		public static IRuleBuilder<T, string> HumanName<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			var options = ruleBuilder.NotNull().WithMessage("Name cannot be null")
				.NotEmpty().WithMessage("Name must be provided")
				.Matches("[A-Za-z]").WithMessage("Name can only contain alphabeths")
				.MinimumLength(2).WithMessage("Name is limited to a minimum of 2 characters")
				.MaximumLength(25).WithMessage("Name is limited to a maximum of 25 characters");

			return options;
		}

		public static IRuleBuilder<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			var options = ruleBuilder.NotNull().WithMessage("PhoneNumber cannot be null")
				.NotEmpty().WithMessage("PhoneNumber must be provided")
				.Matches(@"^[0]\d{10}$").WithMessage("Phone number must start with 0 and must be 11 digits")
				.OverridePropertyName("phone_number");

			return options;
		}

		public static IRuleBuilder<T, string> PassWord<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			var options = ruleBuilder.NotNull().WithMessage("Password cannot be null")
				.NotEmpty().WithMessage("password must be provided")
				.Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@#$%^&+=!])(?!.*\s).+$")
				.WithMessage("password must contain character, letter and Number")
				.MinimumLength(8);

			return options;
		}

	}
}
