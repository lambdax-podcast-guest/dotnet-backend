using System.Net.Http;
using System.Threading.Tasks;
using Guests.Models.Inputs;

namespace Guests.Tests
{
    // -------------------------------------------------------------------------------------------------
    /// <summary>Helpers for testing the account endpoints: Login, Register, Update and Delete User</summary>
    // -------------------------------------------------------------------------------------------------
    public class AccountHelper
    {
        private const string RegisterUri = "/api/account/register";

        private static int Index = 0;

        public static string[] DefaultRoles = new string[] { "Guest", "Host" };

        public AccountHelper() { }

        /// <summary>Generates a new valid register model, with incrementing index property to make sure our unique properties are unique. string[] roles is optional, if you don't include it your user will be registered as both a guest and a host</summary>
        public static RegisterInput GenerateUniqueRegisterModel(string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput guestUser = new RegisterInput() { FirstName = $"Unique{Index}", LastName = $"User{Index}", Roles = roles, Email = $"UniqueUser{Index}@yahoo.com", Password = $"Unique{Index}!" };
            Index++;
            return guestUser;
        }

        /// <summary>Generates a new valid register model, converts it to json content with headers, and registers it to the database. string[] roles is optional, if you don't include it your user will be registered as both a guest and a host</summary>
        public static async Task<HttpResponseMessage> RegisterUniqueRegisterModel(HttpClient client, string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput guestUser = GenerateUniqueRegisterModel(roles);

            var content = JsonHelper.CreatePostContent(guestUser);
            // get the response
            HttpResponseMessage response = await client.PostAsync(RegisterUri, content);
            return response;
        }

        /// <summary>Generates a new valid register model, converts it to json content with headers, and registers it to the database. string[] roles is optional, if you don't include it your user will be registered as both a guest and a host. Logs in the user and returns the response. We will need to register and log in many users to test the authenticated endpoints, this function will keep our code dry. </summary>
        public static async Task<HttpResponseMessage> RegisterAndLogInNewUser(HttpClient client, string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput guestUser = GenerateUniqueRegisterModel(roles);

            var content = JsonHelper.CreatePostContent(guestUser);
            // get the response
            HttpResponseMessage response = await client.PostAsync(RegisterUri, content);

            // now we need a new login model. make one with a bad password
            LoginInput loginInput = new LoginInput() { Email = guestUser.Email, Password = guestUser.Password };

            // get the login Input as Json Content with headers
            var loginContent = JsonHelper.CreatePostContent(loginInput);

            // get the login response
            HttpResponseMessage loginResponse = await client.PostAsync("/api/account/login", loginContent);

            return loginResponse;
        }
    }
}
