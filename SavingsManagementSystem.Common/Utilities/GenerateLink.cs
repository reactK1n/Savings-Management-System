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

	}
}
