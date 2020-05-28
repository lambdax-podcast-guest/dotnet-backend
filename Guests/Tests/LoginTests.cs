using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Guests.Models.Inputs;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Xunit.Abstractions;

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
            // generate new unique user and register it
            RegisterInput guestUser = AccountHelper.GenerateUniqueRegisterModel();

            // turn the register input into json and set the request headers
            JsonContent content = JsonHelper.CreatePostContent(guestUser);

            // get the response
            HttpResponseMessage registerResponse = await fixture.httpClient.PostAsync("/api/account/register", content);

            // now we need a new login model. make one with a bad password
            LoginInput loginInput = new LoginInput() { Email = guestUser.Email, Password = "BadPassword1!" };

            // get the login Input as Json Content with headers
            JsonContent loginContent = JsonHelper.CreatePostContent(loginInput);

            // get the login response
            HttpResponseMessage loginResponse = await fixture.httpClient.PostAsync("/api/account/login", loginContent);

            // I don't believe we should use an assertion scope here: if the register failed, all the other assertions will also fail, which will just much up the output
            // assert the response was successful
            registerResponse.IsSuccessStatusCode.Should().BeTrue("because we registered a unique user: if the register failed then the log in flow is invalid");

            // Assert the login response was not successful
            loginResponse.IsSuccessStatusCode.Should().BeFalse("because we expect the login request to have failed since we provided a bad password");

            // Assert we got a bad request from the login response
            loginResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "because we expect the request to return 400 bad request since the password is invalid");
        }

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test the the login endpoing returns a token</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestLoginReturnsAToken()
        {
            HttpResponseMessage response = await AccountHelper.RegisterAndLogInNewUser(fixture.httpClient);

            LoginOutput resultAsObject = await JsonSerializer.DeserializeAsync<LoginOutput>(response.Content.ReadAsStreamAsync().Result);

            // assert the object we created from the response has a token field and its value is not null
            resultAsObject.token.Should().NotBeNull("because we expect the response object to contain a token field with a value");
        }
        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the login endpoint returns a valid token. If the token is invalid an exception will be thrown, causing the test to fail.</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestLoginReturnsValidToken()
        {
            // register and login a new unique user
            HttpResponseMessage response = await AccountHelper.RegisterAndLogInNewUser(fixture.httpClient);

            // Get the response as an object so we can get the token from it
            LoginOutput resultAsObject = await JsonSerializer.DeserializeAsync<LoginOutput>(response.Content.ReadAsStreamAsync().Result);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // We want to use FluentAssertions to assert that validating the token does not throw, so we need to wrap it in a delegate
            Func<SecurityToken> validateToken = () =>
            {
                tokenHandler.ValidateToken(resultAsObject.token, new TokenValidationParameters
                {
                    ValidIssuer = fixture.Configuration["Guests:JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(fixture.Configuration["Guests:JwtKey"])),
                        ValidateAudience = false
                }, out SecurityToken validatedToken);
                return validatedToken;
            };

            // assert that validateToken did not throw, which means our token is valid
            validateToken.Should().NotThrow("because the token should be valid");
        }
        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the login endpoint returns a token that contains claims for roles, id and email</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestLoginTokenContainsRolesAndUserIdAndEmail()
        {
            // register and login a new unique user
            HttpResponseMessage response = await AccountHelper.RegisterAndLogInNewUser(fixture.httpClient);

            // Get the response as an object so we can get the token from it
            LoginOutput resultAsObject = await JsonSerializer.DeserializeAsync<LoginOutput>(response.Content.ReadAsStreamAsync().Result);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // JwtSecurityTokenHandler.ReadToken will throw an exception if the token is invalid
            // use built in delegate Func to Assert that reading the token does not throw 
            Func<JwtSecurityToken> readToken = () => tokenHandler.ReadToken(resultAsObject.token) as JwtSecurityToken;

            using(new AssertionScope())
            {
                // Assert that read token does not throw an exception: if it throws an exception, that means our token was invalid
                readToken.Should().NotThrow("because the token should be valid in order to check its claims");

                // Get the actual token to check
                JwtSecurityToken securityToken = readToken();

                // user only has one id and email but can have many roles
                securityToken.Claims.Should().ContainSingle(claim => claim.Type == ClaimTypes.Email, "because we expect the token to have a name identifier claim");
                securityToken.Claims.Should().ContainSingle(claim => claim.Type == ClaimTypes.NameIdentifier, "because we expect the token to have an email claim");
                securityToken.Claims.Should().Contain(claim => claim.Type == ClaimTypes.Role, "because we expect the token to have at least one role claim");
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
