using Microsoft.AspNetCore.Routing;

namespace SavingsManagementSystem.Common.Utilities
{
	public class GenerateLink
	{

		private readonly LinkGenerator _linkGenerator;

		public GenerateLink(LinkGenerator linkGenerator)
		{
			_linkGenerator = linkGenerator;
		}

		public virtual string GenerateUrl(string action, string controller, object queryParams)
		{
			// Define the route values for the action
			var routeValues = new { controller, action };

			// Generate the link with route values and query parameters
			var link = _linkGenerator.GetPathByAction(routeValues.action, routeValues.controller, queryParams);

			return link;
		}

		public virtual string GenerateUrl(string action, string controller, object queryParams, string protocol)
		{
			// Define the route values for the action
			var routeValues = new { controller, action };

			// Generate the link with route values and query parameters
			var link = _linkGenerator.GetPathByAction(routeValues.action, routeValues.controller, queryParams);

			// Combine the protocol and link to form the URL
			var url = $"{protocol}://{link}";

			return url;
		}

		//this does not encode your url params
		public virtual string GenerateUrl(string action, string controller, string queryParams)
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
