using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Guests.Tests
{
    /// <summary>
    /// Static class containing test functionality for the Authorize and AuthorizeId attributes to be used on the controllers and actions that are protected by those attributes
    /// </summary>
    public static class AuthHelpers
    {
        /// <summary>
        /// The functionality to test the authorize attribute
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="client"></param>
        public static async Task TestAuthorize(string endpoint, HttpMethod method, HttpClient client)
        {
            // create a new request message
            HttpRequestMessage requestMessageNoHeaders = new HttpRequestMessage(method, endpoint);

            // send the request with no headers
            HttpResponseMessage responseWithoutAuthHeaders = await client.SendAsync(requestMessageNoHeaders);

            // register and login a unique GUEST 
            HttpResponseMessage loginResponse = await AccountHelper.RegisterAndLogInNewUser(client, new string[] { "Guest" });

            // deserialize the response and get the token
            LoginOutput loginOutput = await JsonHelper.TryDeserializeJson<LoginOutput>(loginResponse);
            string token = loginOutput.token;

            // generate a new request with headers this time
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, endpoint);
            AuthenticationHeaderValue authHeader;
            bool isValidHeader = AuthenticationHeaderValue.TryParse($"Bearer {token}", out authHeader);
            requestMessage.Headers.Authorization = authHeader;
            HttpResponseMessage responseWithAuthHeaders = await client.SendAsync(requestMessage);

            using(new AssertionScope())
            {
                responseWithoutAuthHeaders.IsSuccessStatusCode.Should().BeFalse($"because we expect the {method.ToString()} request to the {endpoint} to fail without auth headers");

                responseWithAuthHeaders.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint} to succeed with a valid token on the request headers");
            }

        }
    }
}
