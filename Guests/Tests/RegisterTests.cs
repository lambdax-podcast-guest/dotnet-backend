using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Guests.Models;
using Guests.Models.Inputs;
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

            // assert it is successful
            registerResponse.IsSuccessStatusCode.Should().BeTrue("because we registered a unique user and expect registration to succeed");

            // assert the object we created from the response has a token field and its value is not null
            resultAsObject.token.Should().NotBeNull("because we expect the response object to contain a token field with a value");
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
                secondResponse.IsSuccessStatusCode.Should().BeFalse("because ");

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
        public async void TestRegisterPasswordValidation(RegisterInput guestUser, string errorMessage)
        {
            // turn the register input into json and set the request headers
            var content = JsonHelper.CreatePostContent(guestUser);

            // get the response
            HttpResponseMessage response = await fixture.httpClient.PostAsync("/api/account/register", content);

            // assert it is NOT successful
            Assert.False(response.IsSuccessStatusCode);

            // deserialize the stream and get the errors using our helper function
            Errors errors = await JsonHelper.DeserializeResponseAndReturnErrors(response);

            // Assert the expected error message exists on the error object
            Assert.True(errors.GetType().GetProperty(errorMessage) != null);

        }

        /// <summary>
        /// Test that the user can be found in the database once they are registered
        /// </summary>
        [Fact]
        public async void TestRegisterCreatesUserInDatabase()
        {
            // register a new user
            RegisterInput guestUser = AccountHelper.GenerateUniqueRegisterModel();

            // turn the register input into json and set the request headers
            var content = JsonHelper.CreatePostContent(guestUser);

            // get the response
            HttpResponseMessage response = await fixture.httpClient.PostAsync("/api/account/register", content);

            // assert it is successful
            Assert.True(response.IsSuccessStatusCode);

            // query the database directly for the user we just made
            AppUser[] user = fixture.DbContext.Users.Where(u => u.Email == guestUser.Email).ToArray();

            // assert the query found something and only found one thing
            Assert.NotEmpty(user);
            Assert.True(user.Length == 1);

            // assert the returned object is the right type and contains the expected data
            Assert.IsType(Type.GetType(typeof(AppUser).AssemblyQualifiedName), user[0]);
            Assert.True(user[0].GetType().GetProperty("Email").GetValue(user[0]).ToString() == guestUser.Email);
        }
    }
}
