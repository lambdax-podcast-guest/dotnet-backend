using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Guests.Models.Customizations
{
    public class AuthorizeIdAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public AuthorizeIdAttribute() { }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // If the user is an admin, immediately let them through
            if (context.HttpContext.User.Claims.First(claim => claim.Value == "Admin").Value.Length > 0) return;
            // Grab the path of the request
            string path = context.HttpContext.Request.Path;
            // Split the path and get the last item in the array (would be the id - must be a better way to get this done)
            string userId = path.Split('/').Last();
            // Grab the claim containing the request user's id
            string uid = context.HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            // If the request user's id doesn't match, don't let them through
            if (uid == null || uid != userId) context.Result = new UnauthorizedResult();
        }
    }
}
