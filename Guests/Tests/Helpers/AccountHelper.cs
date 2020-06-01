using System.Net.Http;
using System.Threading.Tasks;
using Guests.Models.Inputs;

namespace Guests.Tests
{
    /// <summary>
    /// Helpers for testing the account endpoints: Login, Register, Update and Delete User
    /// </summary>
    public class AccountHelper
    {
        private const string RegisterUri = "/api/account/register";
        private const string LoginUri = "api/account/login";

        private static int Index = 0;

        public static string[] DefaultRoles = new string[] { "Guest", "Host" };

        public AccountHelper() { }

        /// <summary>
        /// Registers a given user with the given client. 
        /// </summary>
        /// <param name="client">HttpClient used to make the request</param>
        /// <param name="user">The User you would like to register</param>
        public static async Task<HttpResponseMessage> RegisterUser(HttpClient client, RegisterInput user)
        {
            var content = JsonHelper.CreatePostContent(user);

            HttpResponseMessage response = await client.PostAsync(RegisterUri, content);

            return response;
        }

        /// <summary>
        /// Generates a new valid register model, with incrementing index property to make sure our unique properties are unique.
        /// </summary>
        /// <param name="roles">The roles for the user you are generating in a string array. If you don't include it your user will be generated as both a guest and a host</param>
        public static RegisterInput GenerateUniqueRegisterModel(string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput user = new RegisterInput() { FirstName = $"Unique{Index}", LastName = $"User{Index}", Roles = roles, Email = $"UniqueUser{Index}@yahoo.com", Password = $"Unique{Index}!" };
            Index++;
            return user;
        }

        /// <summary>
        /// Generates a new valid register model, converts it to json content with headers, and registers it to the database. 
        /// </summary>
        /// <param name="client">HttpClient used to make the request</param>
        /// <param name="roles">The roles for the user you are registering in a string array. If you don't include it your user will be registered as both a guest and a host</param>
        public static async Task<HttpResponseMessage> RegisterUniqueRegisterModel(HttpClient client, string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput user = GenerateUniqueRegisterModel(roles);

            // get the response
            HttpResponseMessage response = await RegisterUser(client, user);

            return response;
        }

        /// <summary>
        /// Generates a new valid register model, converts it to json content with headers, and registers it to the database. string[] roles is optional, if you don't include it your user will be registered as both a guest and a host. Logs in the user and returns the response. We will need to register and log in many users to test the authenticated endpoints, this function will keep our code dry. 
        /// </summary>
        /// <param name="client">HttpClient used to make the request</param>
        /// <param name="roles">The roles for the user you are registering in a string array. If you don't include it your user will be registered as both a guest and a host</param>
        public static async Task<HttpResponseMessage> RegisterAndLogInNewUser(HttpClient client, string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput user = GenerateUniqueRegisterModel(roles);

            // get the response
            HttpResponseMessage response = await RegisterUser(client, user);

            // now we need a new login model. make one with a bad password
            LoginInput loginInput = new LoginInput() { Email = user.Email, Password = user.Password };

            // get the login Input as Json Content with headers
            var loginContent = JsonHelper.CreatePostContent(loginInput);

            // get the login response
            HttpResponseMessage loginResponse = await client.PostAsync(LoginUri, loginContent);

            return loginResponse;
        }
    }
}
