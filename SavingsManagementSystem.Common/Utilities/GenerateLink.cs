using Microsoft.AspNetCore.Routing;

namespace SavingsManagementSystem.Common.Utilities
{
	public static class LinkGenerator
	{
		//this does not encode your url params
		public static string GenerateUrl(string action, string controller, string queryParams)
		{
			// Define the route values for the action
			var routeValues = new { controller, action };

			// Combine the route values and query parameters
			var link = $"/{routeValues.controller}/{routeValues.action}?{queryParams}";

			// Add "https://" to the beginning of the URL
			var url = $"https://{link}";

			return url;
		}

	}
}
