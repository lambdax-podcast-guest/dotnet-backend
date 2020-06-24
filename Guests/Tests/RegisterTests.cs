using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using FluentAssertions.Execution;
using Guests.Models;
using Guests.Models.Inputs;
using Guests.Tests.DataFixtures;
using Guests.Tests.Helpers;
using Guests.Tests.ReusableFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Guests.Tests
{
    public class RegisterTests : TestBaseWithFixture
    {
        public RegisterTests(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        /// <summary>
        /// Test that the register endpoint returns a token when the request is successful
        /// </summary>
        [Fact]
        public async void TestRegisterReturnsToken()
        {
            // generate and register a user and get the response
            HttpResponseMessage registerResponse = await AccountHelper.RegisterUniqueRegisterModel(fixture.httpClient);

            // deserialize the response content
            RegisterOutput resultAsObject = await JsonHelper.TryDeserializeJson<RegisterOutput>(registerResponse);

            using(new AssertionScope())
            {
                // assert it is successful
                registerResponse.IsSuccessStatusCode.Should().BeTrue("because we registered a unique user and expect registration to succeed");

                // assert the object we created from the response has a token field and its value is not null
                resultAsObject.token.Should().NotBeNull("because we expect the response object to contain a token field with a value");

            }
        }

        /// <summary>
        /// Test that the register endpoint returns a bad request if someone tries to register with the same email twice
        /// </summary>
        [Fact]
        public async void TestRegisterReturnsBadRequestIfEmailExists()
        {
            // generate a new unique user to 
            RegisterInput user = AccountHelper.GenerateUniqueRegisterModel();

            // get the response
            HttpResponseMessage firstResponse = await AccountHelper.RegisterUser(fixture.httpClient, user);

            // run the same request again, we should get 400 this time
            HttpResponseMessage secondResponse = await AccountHelper.RegisterUser(fixture.httpClient, user);

            // use an assertion scope so we get output for every failed assertion
            using(new AssertionScope())
            {

                // the first time we run the request the response should be successful
                firstResponse.IsSuccessStatusCode.Should().BeTrue("because the first time we run the register request it should be successful since this user was unique.");

                // assert it is NOT successful
                secondResponse.IsSuccessStatusCode.Should().BeFalse("because the second time we run the register request it should *NOT* be successful since the user is not unique.");

                // deserialize the response and get the errors using our helper function
                Errors errors = await JsonHelper.DeserializeResponseAndReturnErrors(secondResponse);

                // Assert that the duplicate email error exists on the errors field
                errors.DuplicateEmail.Should().NotBeNull("because we expect the errors field to contain an entry with a value for the duplicate email error, since we tried to register the exact same user twice.");

            }
        }

        /// <summary>
        /// Test that the register endpoint returns a bad request on weak passwords. 
        /// The default password validation from Identity requires the password have an uppercase char, a lowercase char, a digit, and a non-alphanumeric char, and must also be at least six characters long
        /// </summary>
        [Theory]
        [ClassData(typeof(BadPasswordUsers))]
        public async void TestRegisterPasswordValidation(RegisterInput user, string errorMessage)
        {
            // get the response
            HttpResponseMessage registerResponse = await AccountHelper.RegisterUser(fixture.httpClient, user);

            using(new AssertionScope())
            {
                // assert it is NOT successful
                registerResponse.IsSuccessStatusCode.Should().BeFalse("because we expect identity to reject bad passwords");

                // deserialize the stream and get the errors using our helper function
                Errors errors = await JsonHelper.DeserializeResponseAndReturnErrors(registerResponse);

                // Assert the expected error message exists on the error object
                errors.GetType().GetProperty(errorMessage).Should().NotBeNull($"because we expect a user with each type of bad password to return a different error message, this password: {user.Password} should return this message: {errorMessage}");
            }
        }

        /// <summary>
        /// Test that the user can be found in the database once they are registered
        /// </summary>
        [Fact]
        public async void TestRegisterCreatesUserInDatabase()
        {
            // register a new user
            RegisterInput registerUser = AccountHelper.GenerateUniqueRegisterModel();

            // get the response
            HttpResponseMessage registerResponse = await AccountHelper.RegisterUser(fixture.httpClient, registerUser);

            // query the database directly for the user we just made
            AppUser[] user = fixture.DbContext.Users.Where(u => u.Email == registerUser.Email).ToArray();

            using(new AssertionScope())
            {
                // assert the response was successful
                registerResponse.IsSuccessStatusCode.Should().BeTrue("because we registered a unique user, and if the response was not successful the entity should not be in the database");

                // assert the query found something and only found one thing
                user.Should().ContainSingle("because all of our users emails are unique, so we should only find one entry per email");

                // assert the returned object is the right type and contains the expected data
                user[0].Should().BeOfType<AppUser>("because we expect to get the app user type from the user table");
                (user[0].GetType().GetProperty("Email").GetValue(user[0]).ToString() == registerUser.Email).Should().BeTrue("because we expect our query to return the entry we registered");
            }
        }
    }
}
