using Guests.Models.Inputs;
using System.Net.Http;
using System.Threading.Tasks;

namespace Guests.Tests
{
    // -------------------------------------------------------------------------------------------------
    /// <summary>Helpers for testing the account endpoints: Login, Register, Update and Delete User. Create an instance of this class in the database fixture to be shared with the tests</summary>
    // -------------------------------------------------------------------------------------------------
    public class AccountHelper
    {
        public int Index { get; private set; }

        public string[] DefaultRoles { get; private set; }

        public AccountHelper()
        {
            Index = 0;
            DefaultRoles = new string[] { "Guest", "Host" };
        }

        /// <summary>Generates a new valid register model, with incrementing index property to make sure our unique properties are unique. string[] roles is optional, if you don't include it your user will be registered as both a guest and a host</summary>
        public RegisterInput GenerateUniqueRegisterModel(string[] roles = null)
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
        public async Task<HttpResponseMessage> RegisterUniqueRegisterModel(HttpClient client, string[] roles = null)
        {
            if (roles == null)
            {
                roles = DefaultRoles;
            }
            RegisterInput guestUser = GenerateUniqueRegisterModel(roles);

            var content = JsonHelper.CreatePostContent(guestUser);
            // get the response
            HttpResponseMessage response = await client.PostAsync("/api/account/register", content);
            return response;
        }
    }
}