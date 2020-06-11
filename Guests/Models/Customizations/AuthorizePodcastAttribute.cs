using System;
using System.Linq;
using System.Security.Claims;
using Guests.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Guests.Models.Customizations
{
    public class AuthorizePodcastAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public AuthorizePodcastAttribute() { }
        public void OnAuthorization(AuthorizationFilterContext authContext)
        {
            // if user is admin, automatically authorize
            if (authContext.HttpContext.User.Claims.Any(role => role.Type == ClaimTypes.Role && role.Value == Role.Admin)) return;
            // get db context
            AppUserContext context = (AppUserContext) authContext.HttpContext.RequestServices.GetService(typeof(AppUserContext));
            // get id of podcast
            string pid = authContext.HttpContext.Request.Path.ToString().Split("/").Last();
            // get user's id
            string uid = authContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // represents whether or not the user's id matches a host's id of the podcast
            bool isHost = context.PodcastHosts.Where(ph => ph.PodcastId.ToString() == pid).Any(ph => ph.HostId == uid);
            // if not host, unauthorize the result
            if (!isHost) authContext.Result = new UnauthorizedResult();
        }
    }
}
