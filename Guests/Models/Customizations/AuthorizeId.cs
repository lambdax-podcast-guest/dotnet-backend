using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Guests.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Guests.Models.Customizations
{
    public class AuthorizeIdAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        // Initialize list with Admin
        public List<string> requiredRoles = new List<string> { Role.Admin };
        public AuthorizeIdAttribute() { }
        public AuthorizeIdAttribute(params string[] roles)
        {
            // Add each role from params to requiredRoles
            foreach (string role in roles) requiredRoles.Add(role);
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Get all of the user's roles
            Claim[] roles = context.HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).ToArray();
            // If the user has a role that matches any of the roles in requiredRoles, immediately let them through
            if (Array.Exists(roles, role => Array.Exists(requiredRoles.ToArray(), reqRole => role.Value == reqRole))) return;
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
