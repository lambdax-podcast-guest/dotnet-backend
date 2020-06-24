using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Guests.Tests.DataFixtures;
using Guests.Tests.Helpers;
using Guests.Tests.ReusableFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Guests.Tests
{
    public class AuthorizeAttributeTests : TestBaseWithFixture
    {
        public AuthorizeAttributeTests(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output) { }
        /// <summary>
        /// Theory that tests various endpoints and methods that are protected by the authorize attribute
        /// </summary>
        /// <param name="endpoint">The endpoint to test for the Authorize Attribute</param>
        /// <param name="method">The method to run on that endpoint</param>
        [Theory]
        [ClassData(typeof(EndpointsAndMethodsForAuthorize))]
        public async Task TestAuthorize(string endpoint, HttpMethod method)
        {
            // create a new request message
            HttpRequestMessage requestMessageNoHeaders = new HttpRequestMessage(method, endpoint);

            // send the request with no headers
            HttpResponseMessage responseWithoutAuthHeaders = await fixture.httpClient.SendAsync(requestMessageNoHeaders);

            // register and login a unique GUEST 
            HttpResponseMessage loginResponse = await AccountHelper.RegisterAndLogInNewUser(fixture.httpClient, new string[] { "Guest" });

            // deserialize the response and get the token
            LoginOutput loginOutput = await JsonHelper.TryDeserializeJson<LoginOutput>(loginResponse);
            string token = loginOutput.token;

            // generate a new request with headers this time
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, endpoint);
            AuthenticationHeaderValue authHeader;
            bool isValidHeader = AuthenticationHeaderValue.TryParse($"Bearer {token}", out authHeader);
            requestMessage.Headers.Authorization = authHeader;
            HttpResponseMessage responseWithAuthHeaders = await fixture.httpClient.SendAsync(requestMessage);

            using(new AssertionScope())
            {
                responseWithoutAuthHeaders.IsSuccessStatusCode.Should().BeFalse($"because we expect the {method.ToString()} request to the {endpoint} to fail without auth headers");

                responseWithAuthHeaders.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint} to succeed with a valid token on the request headers");
            }
        }

    }
}
