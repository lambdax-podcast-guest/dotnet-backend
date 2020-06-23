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
    public class AuthorizeIdAttributeTests : TestBaseWithFixture
    {
        public AuthorizeIdAttributeTests(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        public async Task TestAuthorizeIdBlocksRequestWithNoHeaders(string endpoint, HttpMethod method)
        {

            string nonOwnerId = await AuthHelper.GenerateNonOwnerId(fixture);
            // create a new request message
            HttpRequestMessage requestMessageNoHeaders = new HttpRequestMessage(method, endpoint + nonOwnerId);

            // send the request with no headers
            HttpResponseMessage responseWithoutAuthHeaders = await fixture.httpClient.SendAsync(requestMessageNoHeaders);

            // assert the request failed
            responseWithoutAuthHeaders.IsSuccessStatusCode.Should().BeFalse($"because we expect the {method.ToString()} request to the {endpoint} to fail without auth headers");
        }

        // public async Task TestAuthorizeIdAllowsProvidedRolesAndAdmin(string endpoint, HttpMethod method, string nonOwnerId, List<AuthHelper.TokenAsObject> tokensAsObjects)
        // {
        //     // generate new requests with the tokens on the headers this time
        //     // whatever roles we passed in are the roles we expect to be authorized
        //     // use the non owner id since we are testing strictly for roles right now
        //     foreach (var tokenObject in tokensAsObjects)
        //     {
        //         HttpRequestMessage requestMessage = new HttpRequestMessage(method, endpoint + nonOwnerId);
        //         AuthenticationHeaderValue authHeader;
        //         bool isValidHeader = AuthenticationHeaderValue.TryParse($"Bearer {tokenObject.Token}", out authHeader);
        //         requestMessage.Headers.Authorization = authHeader;
        //         HttpResponseMessage responseWithAuthHeaders = await fixture.httpClient.SendAsync(requestMessage);
        //         responseWithAuthHeaders.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint} at any id to succeed when the role is {tokenObject.Role}");
        //     }
        // }

        public async Task TestAuthorizeIdAllowsOwner()
        {

        }

        public async Task TestAuthorizeIdBlocksNonOwner()
        {

        }

        public async Task TestAuthorizeIdBlocksNonProvidedRolesThatAreNonOwners()
        {

        }
        /// <summary>
        /// Override this method on the derived class with [MemberData] or [ClassData] to test your endpoints that use the AuthorizeId attribute
        /// </summary>
        /// <param name="endpoint">The endpoint to test for the AuthorizeId attribute</param>
        /// <param name="method">The method to run on that endpoint</param>
        /// <param name="roles">The roles that are expected to pass the endpoint</param>
        // public virtual async Task AuthorizeAttributeIdTests(string endpoint, HttpMethod method, string[] roles)
        // {
        //     // we need a user id for the end of this endpoint since authorize id is used to authorize a resource that belongs to a particular user id
        //     // we'll reuse this id for non owners to assert that the attribute rejects the request
        //     // the role doesn't matter, we won't use this user again, just their id

        //     // register and login unique users for each role we need to authorize on the endpoint
        //     List<AuthHelper.ResponseAsObject> loginResponses = new List<AuthHelper.ResponseAsObject>();

        //     // The authorizeId attribute should also allow admins to pass so also register an admin
        //     HttpResponseMessage adminMessage = await AccountHelper.RegisterUniqueRegisterModel(fixture.httpClient, new string[] { "Admin" });
        //     loginResponses.Add(new AuthHelper.ResponseAsObject() { Message = adminMessage, Role = "Admin" });

        //     foreach (string role in roles)
        //     {
        //         HttpResponseMessage message = await AccountHelper.RegisterUniqueRegisterModel(fixture.httpClient, new string[] { role });
        //         loginResponses.Add(new AuthHelper.ResponseAsObject() { Message = message, Role = role });
        //     }

        //     // deserialize each response and get the tokens
        //     List<AuthHelper.TokenAsObject> tokensAsObjects = new List<AuthHelper.TokenAsObject>();
        //     foreach (var response in loginResponses)
        //     {
        //         RegisterOutput registerOutput = await JsonHelper.TryDeserializeJson<RegisterOutput>((HttpResponseMessage) response.Message);
        //         tokensAsObjects.Add(new AuthHelper.TokenAsObject() { Token = registerOutput.token, Role = response.Role, Id = registerOutput.id });
        //     }

        //     // using(new AssertionScope())
        //     // {

        //     //     foreach (var responseWithAuthHeaders in responsesWithAuthHeaders)
        //     //     {
        //     //         responseWithAuthHeaders.Message.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint} to succeed with a valid token on the request headers");
        //     //     }

        //     // }
        // }

    }
}
