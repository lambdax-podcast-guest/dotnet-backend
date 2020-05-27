using System.Net;
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
        /// <summary>Test that the login endpoint returns bad request when the user logs in with an incorrect password</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestLoginRejectsBadPassword()
        {
            string[] roles = new string[] { "Guest" };

            // generate new unique user and register it
            RegisterInput guestUser = fixture.accountHelper.GenerateUniqueRegisterModel(roles);

            // turn the register input into json and set the request headers
            var content = JsonHelper.CreatePostContent(guestUser);

            // get the response
            HttpResponseMessage response = await fixture.httpClient.PostAsync("/api/account/register", content);

            // assert the response was successful
            Assert.True(response.IsSuccessStatusCode);

            // now we need a new login model. make one with a bad password
            LoginInput loginInput = new LoginInput() { Email = guestUser.Email, Password = "BadPassword1!" };

            // get the login Input as Json Content with headers
            var loginContent = JsonHelper.CreatePostContent(loginInput);

            // get the login response
            HttpResponseMessage loginResponse = await fixture.httpClient.PostAsync("/api/account/login", loginContent);

            // Assert the login response was not successful
            Assert.False(loginResponse.IsSuccessStatusCode);

            // Assert we got a bad request from the login response
            Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
        }

        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test the the login endpoing returns a token</summary>
        // // -------------------------------------------------------------------------------------------------
        // [Fact]
        // public async void TestLoginReturnsAToken()
        // {
        // }
        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test that the login endpoint rejects requests that have missing fields</summary>
        // // -------------------------------------------------------------------------------------------------
        // [Fact]
        // public async void TestLoginReturnsBadRequesOnInvalidModel()
        // {

        // }
    }
}