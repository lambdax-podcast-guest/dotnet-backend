using Xunit;
using Xunit.Abstractions;
using Guests.Models.Inputs;
using System.Net.Http;

namespace Guests.Tests
{
    public class LoginTests : TestBaseWithFixture
    {
        public LoginTests(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the login endpoint returns Unauthorized when the user logs in with an incorrect password</summary>
        // -------------------------------------------------------------------------------------------------
        public async void TestLoginRejectsBadPassword()
        {
            string[] roles = new string[] { "Guest" };

            // generate new unique user and register it
            // TODO!! This logic should be wrapped up somewhere, it is heavilly repeated between register tests and login tests. Also since we are sharing db context among all the tests we need to be in strict control of what data we have called
            RegisterInput guestUser = fixture.accountHelper.GenerateUniqueRegisterModel(roles);

            // turn the register input into json and set the request headers
            var content = JsonHelper.CreatePostContent(guestUser);

            // get the response
            HttpResponseMessage firstResponse = await fixture.httpClient.PostAsync("/api/account/register", content);

            // the first time we run the request the response should be successful
            Assert.True(firstResponse.IsSuccessStatusCode);

        }

        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test the the login endpoing returns a token</summary>
        // // -------------------------------------------------------------------------------------------------

        // public async void TestLoginReturnsAToken()
        // {

        // }
        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test that the login endpoint rejects requests that have missing fields</summary>
        // // -------------------------------------------------------------------------------------------------

        // public async void TestLoginReturnsBadRequesOnInvalidModel()
        // {

        // }
    }
}