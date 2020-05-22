using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Guests.Models.Inputs;
using Xunit;
using Xunit.Abstractions;

namespace GuestTests
{
    [Collection("DbCollection")]
    public class AccountTests
    {
        DatabaseFixture fixture;
        private readonly ITestOutputHelper outputter;
        public AccountTests(DatabaseFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            // Use output.WriteLine to print to console
            // This ITestOutputHelper class only knows how to use the Visual Studio Output though, so to tell it to use the console here in VSCode, run the test command like this:
            // dotnet test -l "console;verbosity=detailed"
            outputter = output;
        }

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the register endpoint returns a token when the request is successful</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestRegisterReturnsToken()
        {
            // generate an array of roles for our fake user
            string[] roles = new string[] { "Guest" };

            RegisterInput guestUser = new RegisterInput() { FirstName = "Bob", LastName = "Ross", Roles = roles, Email = "BobRoss@yahoo.com", Password = "HappyLittleMistakes1!" };

            // turn the register input into json and set the request headers
            var content = JsonContent.Create(guestUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // get the response
            HttpResponseMessage response = await fixture.httpClient.PostAsync("/api/account/register", content);

            // assert it is successful
            Assert.True(response.IsSuccessStatusCode);

            // deserialize the string into what we expect the output to look like
            RegisterOutput resultAsObject = await JsonSerializer.DeserializeAsync<RegisterOutput>(response.Content.ReadAsStreamAsync().Result);

            // assert the object we created from the response has a token field and its value is not null
            Assert.True(resultAsObject.token != null);

        }

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the register endpoint returns a bad request if someone tries to register with the same email twice</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestRegisterReturnsBadRequestIfEmailExists()
        {
            string[] roles = new string[] { "Guest" };

            // register the same user as in the last request: it exists in the db, so we should get 400 back
            RegisterInput guestUser = new RegisterInput() { FirstName = "Bob", LastName = "Ross", Roles = roles, Email = "BobRoss@yahoo.com", Password = "HappyLittleMistakes1!" };

            // turn the register input into json and set the request headers
            var content = JsonContent.Create(guestUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // get the response
            HttpResponseMessage response = await fixture.httpClient.PostAsync("/api/account/register", content);

            // assert it is NOT successful
            Assert.False(response.IsSuccessStatusCode);

            // deserialize the stream and get the errors using our helper function
            Errors errors = await JsonHelper.DeserializeResponseAndReturnErrors(response);

            // Assert that the duplicate email error exists on the errors field
            Assert.True(errors.DuplicateEmail != null);
        }

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the register endpoint returns a bad request on weak passwords. The default password validation from Identity requires the password have an uppercase char, a lowercase char, a digit, and a non-alphanumeric char, and must also be at least six characters long</summary>
        // -------------------------------------------------------------------------------------------------
        [Theory]
        [ClassData(typeof(BadPasswordUsers))]
        public async void TestRegisterPasswordValidation(RegisterInput guestUser, string errorMessage)
        {
            // turn the register input into json and set the request headers
            var content = JsonContent.Create(guestUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // get the response
            HttpResponseMessage response = await fixture.httpClient.PostAsync("/api/account/register", content);

            // assert it is NOT successful
            Assert.False(response.IsSuccessStatusCode);

            // deserialize the stream and get the errors using our helper function
            Errors errors = await JsonHelper.DeserializeResponseAndReturnErrors(response);

            // Assert the expected error message exists on the error object
            Assert.True(errors.GetType().GetProperty(errorMessage) != null);
        }
    }
}
