using System.Net;
using Xunit;
using Xunit.Abstractions;
using Guests.Models.Inputs;
using System.Net.Http;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test the the login endpoing returns a token</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestLoginReturnsAToken()
        {
            HttpResponseMessage response = await fixture.accountHelper.RegisterAndLogInNewUser(fixture.httpClient);

            LoginOutput resultAsObject = await JsonSerializer.DeserializeAsync<LoginOutput>(response.Content.ReadAsStreamAsync().Result);

            // assert the object we created from the response has a token field and its value is not null
            Assert.True(resultAsObject.token != null);
        }
        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the login endpoint returns a valid token</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestLoginReturnsValidToken()
        {
            // register and login a new unique user
            HttpResponseMessage response = await fixture.accountHelper.RegisterAndLogInNewUser(fixture.httpClient);

            // Get the response as an object so we can get the token from it
            LoginOutput resultAsObject = await JsonSerializer.DeserializeAsync<LoginOutput>(response.Content.ReadAsStreamAsync().Result);

            var tokenHandler = new JwtSecurityTokenHandler();

            // If the token is not valid there is a number of different exceptions to be thrown
            // So rather than just let those unhandled exceptions go to the test output, we will actually try and catch them, and provide a custom failure message
            try
            {
                tokenHandler.ValidateToken(resultAsObject.token, new TokenValidationParameters
                {
                    ValidIssuer = fixture.Configuration["Guests:JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(fixture.Configuration["Guests:JwtKey"])),
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception err)
            {
                // xUnit doesn't have an Assert.Fail, their recommended method is to Assert.True(false, message);
                Assert.True(false, $"{err.GetType().ToString()}: {err.Message.ToString()}");
            }

        }
        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test that the login endpoint rejects requests that have missing fields</summary>
        // // -------------------------------------------------------------------------------------------------
        // [Fact]
        // public async void TestLoginReturnsBadRequesOnInvalidModel()
        // {

        // }
    }
}