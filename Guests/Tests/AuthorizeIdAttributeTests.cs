using System.Collections.Generic;
using System.Net;
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
    /// <summary>
    /// Tests for the AuthorizeId attributes. Add your endpoints and methods to the Class Data classes to test those endpoints that are protected by the AuthorizeId attribute for different factors
    /// </summary>
    public class AuthorizeIdAttributeTests : TestBaseWithFixture, IClassFixture<NonOwnerIdFixtureWithUsers>
    {
        public string NonOwnerId;

        public Dictionary<string, AuthHelper.TestUser> testUsers;
        public AuthorizeIdAttributeTests(DatabaseFixture fixture, ITestOutputHelper output, NonOwnerIdFixtureWithUsers NonOwnerIdFixtureWithUsers) : base(fixture, output)
        {
            NonOwnerId = NonOwnerIdFixtureWithUsers.nonOwnerId;
            testUsers = NonOwnerIdFixtureWithUsers.testUsers;
        }

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
            // create a new request message
            HttpRequestMessage requestMessageNoHeaders = new HttpRequestMessage(method, endpoint + "/" + NonOwnerId);

            // if we've been provided a body for put or post add it here
            if (body != null)
            {
                requestMessageNoHeaders.Content = JsonHelper.CreatePostContent(body);
            }

            // send the request with no headers
            HttpResponseMessage authIdTestResponseWithoutAuthHeaders = await fixture.httpClient.SendAsync(requestMessageNoHeaders);

            // assert the request failed
            authIdTestResponseWithoutAuthHeaders.IsSuccessStatusCode.Should().BeFalse($"because we expect the {method.ToString()} request to the {endpoint + "/id"} to fail without auth headers");
        }

        /// <summary>
        /// Tests that AuthorizeId allows the provided roles to access the resource.
        /// </summary>
        /// <param name="endpoint">The endpoint to run the request on</param>
        /// <param name="method">The method to run on the endpoint</param>
        /// <param name="roles">The roles that should be allowed to pass that endpoint</param>
        /// <param name="body">The body for a Put or Post request, pass null for Get and Delete</param>
        /// <returns></returns>
        [Theory]
        [ClassData(typeof(EndpointsAndMethodsForRolesTestOnAuthorizeId))]
        public async Task TestAuthorizeIdAllowsProvidedRolesAndAdmin(string endpoint, HttpMethod method, string[] roles, object body)
        {
            // generate new requests with the tokens on the headers this time
            // whatever roles we passed in are the roles we expect to be authorized
            // use the non owner id since we are testing strictly for roles right now
            // we need to get just the test users that should be able to pass this endpoint
            List<AuthHelper.TestUser> users = new List<AuthHelper.TestUser>();
            foreach (string role in roles)
            {
                users.Add(testUsers[role]);
            }

            // send a request to our endpoint with the users tokens, but request resources those users do not own in order to check the role based auth
            List<AuthHelper.ResponseAsObject> responsesWithAuthHeaders = new List<AuthHelper.ResponseAsObject>();
            foreach (AuthHelper.TestUser user in users)
            {
                AuthHelper.ResponseAsObject response = await AuthHelper.GenerateAuthIdRequest(endpoint + "/" + NonOwnerId, method, user, fixture, body);
                responsesWithAuthHeaders.Add(response);
            }
            using(new AssertionScope())
            {

                foreach (var authIdTestResponseWithAuthHeaders in responsesWithAuthHeaders)
                {
                    authIdTestResponseWithAuthHeaders.Message.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint + "/id"} to succeed when a/n {authIdTestResponseWithAuthHeaders.Role} is making a request");
                }

            }
        }

        public async Task TestAuthorizeIdAllowsOwner()
        {

        }

        // public async Task TestAuthorizeIdBlocksNonOwner()
        // {

        // }

        // public async Task TestAuthorizeIdBlocksNonProvidedRolesThatAreNonOwners()
        // {

        // }

    }
}
