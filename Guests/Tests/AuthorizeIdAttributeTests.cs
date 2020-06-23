using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace Guests.Tests
{
    /// <summary>
    /// Tests for the AuthorizeId attributes. Add your endpoints and methods to the Class Data classes to test those endpoints that are protected by the AuthorizeId attribute for different factors
    /// </summary>
    public class AuthorizeIdAttributeTests : TestBaseWithFixture
    {
        public AuthorizeIdAttributeTests(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        /// <summary>
        /// Test that endpoints decorated with AuthorizeId endpoint rejects requests that have no headers
        /// </summary>
        /// <param name="endpoint">The endpoint to test</param>
        /// <param name="method">The method to use on the endpoint</param>
        /// <param name="body">Body for Put and Post: pass null for Get and Delete</param>
        [Theory]
        [ClassData(typeof(EndpointsAndMethodsForNoHeadersTestsOnAuthorizeId))]
        public async Task TestAuthorizeIdRejectsRequestWithNoHeaders(string endpoint, HttpMethod method, object body)
        {
            string nonOwnerId = await AuthHelper.GenerateNonOwnerId(fixture);

            // create a new request message
            HttpRequestMessage requestMessageNoHeaders = new HttpRequestMessage(method, endpoint + "/" + nonOwnerId);

            // if we've been provided a body for put or post add it here
            if (body != null)
            {
                requestMessageNoHeaders.Content = JsonHelper.CreatePostContent(body);
            }

            // send the request with no headers
            HttpResponseMessage responseWithoutAuthHeaders = await fixture.httpClient.SendAsync(requestMessageNoHeaders);

            // assert the request failed
            responseWithoutAuthHeaders.IsSuccessStatusCode.Should().BeFalse($"because we expect the {method.ToString()} request to the {endpoint} to fail without auth headers");
        }

        [Theory]
        [ClassData(typeof(EndpointsAndMethodsForRolesTestOnAuthorizeId))]
        public async Task TestAuthorizeIdAllowsProvidedRolesAndAdmin(string endpoint, HttpMethod method, string[] roles, object body)
        {
            // generate new requests with the tokens on the headers this time
            // whatever roles we passed in are the roles we expect to be authorized
            // use the non owner id since we are testing strictly for roles right now
            string nonOwnerId = await AuthHelper.GenerateNonOwnerId(fixture);

            // register and login unique users for each role we need to authorize on the endpoint
            List<AuthHelper.ResponseAsObject> loginResponses = new List<AuthHelper.ResponseAsObject>();

            // The authorizeId attribute should also allow admins to pass so also register an admin
            HttpResponseMessage adminMessage = await AccountHelper.RegisterUniqueRegisterModel(fixture.httpClient, new string[] { "Admin" });
            loginResponses.Add(new AuthHelper.ResponseAsObject() { Message = adminMessage, Role = "Admin" });

            foreach (string role in roles)
            {
                HttpResponseMessage message = await AccountHelper.RegisterUniqueRegisterModel(fixture.httpClient, new string[] { role });
                loginResponses.Add(new AuthHelper.ResponseAsObject() { Message = message, Role = role });
            }

            // deserialize each response and get the tokens
            List<AuthHelper.TokenAsObject> tokensAsObjects = new List<AuthHelper.TokenAsObject>();
            foreach (var response in loginResponses)
            {
                RegisterOutput registerOutput = await JsonHelper.TryDeserializeJson<RegisterOutput>((HttpResponseMessage) response.Message);
                tokensAsObjects.Add(new AuthHelper.TokenAsObject() { Token = registerOutput.token, Role = response.Role, Id = registerOutput.id });
            }

            // send a request to our endpoint with the tokens we just acquired
            List<AuthHelper.ResponseAsObject> responsesWithAuthHeaders = new List<AuthHelper.ResponseAsObject>();
            foreach (var tokenObject in tokensAsObjects)
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(method, endpoint + "/" + nonOwnerId);
                if (body != null)
                {
                    requestMessage.Content = JsonHelper.CreatePostContent(body);
                }
                AuthenticationHeaderValue authHeader;
                bool isValidHeader = AuthenticationHeaderValue.TryParse($"Bearer {tokenObject.Token}", out authHeader);
                requestMessage.Headers.Authorization = authHeader;
                HttpResponseMessage responseWithAuthHeaders = await fixture.httpClient.SendAsync(requestMessage);
                responsesWithAuthHeaders.Add(new AuthHelper.ResponseAsObject() { Message = responseWithAuthHeaders, Role = tokenObject.Role });
            }
            using(new AssertionScope())
            {

                foreach (var responseWithAuthHeaders in responsesWithAuthHeaders)
                {
                    responseWithAuthHeaders.Message.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint} to succeed when a/n {responseWithAuthHeaders.Role} is making a request");
                }

            }
        }

        // public async Task TestAuthorizeIdAllowsOwner()
        // {

        // }

        // public async Task TestAuthorizeIdBlocksNonOwner()
        // {

        // }

        // public async Task TestAuthorizeIdBlocksNonProvidedRolesThatAreNonOwners()
        // {

        // }

    }
}
