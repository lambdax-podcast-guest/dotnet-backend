using System.Text.Json;
using Xunit;
using Guests.Models.Inputs;
using Xunit.Abstractions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

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

        // use this to deserialize register output
        public class RegisterOutput
        {
            public string id { get; set; }
            public string token { get; set; }
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

            // await the async action of getting the response content as a string
            string responseString = await response.Content.ReadAsStringAsync();

            // deserialize the string into what we expect the output to look like
            RegisterOutput resultAsObject = JsonSerializer.Deserialize<RegisterOutput>(responseString);

            // assert the object we created from the response has a token field and its value is not null
            Assert.True(resultAsObject.token != null);

        }

        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test that the register endpoint returns a bad request if someone tries to register with the same email twice</summary>
        // // -------------------------------------------------------------------------------------------------
        // [Fact]
        // public async void TestRegisterReturnsBadRequestIfEmailExists()
        // {
        //     var controller = fixture.accountController;
        //     string[] roles = new string[] { "Guest" };

        //     // register the same user as in the last request: it exists in the db, so we should get 400 back
        //     RegisterInput guestUser = new RegisterInput() { FirstName = "Bob", LastName = "Ross", Roles = roles, Email = "BobRoss@yahoo.com", Password = "HappyLittleMistakes1!" };

        //     var result = await controller.Register(guestUser);

        //     // this will assert that the response returned a BadRequestObjectResult, and if it did it will cast our result (which is an IActionResult) to a BadRequestObjectResult
        //     BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        // }

        // // -------------------------------------------------------------------------------------------------
        // /// <summary>Test that the register endpoint returns a bad request on weak passwords. The default password validation from Identity requires the password have an uppercase char, a lowercase char, a digit, and a non-alphanumeric char, and must also be at least six characters long</summary>
        // // -------------------------------------------------------------------------------------------------
        // [Fact]
        // public async void TestRegisterPasswordValidation()
        // {
        //     var controller = fixture.accountController;
        //     string[] roles = new string[] { "Guest" };

        //     // All these users have bad passwords, but their emails are unique and their roles exist, so the only reason we should get 400 is for their bad passwords
        //     RegisterInput passwordTooShortUser = new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal1@yahoo.com", Password = "Tes1!" };

        //     RegisterInput noUpperLetterUser = new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal2@yahoo.com", Password = "test1!" };

        //     RegisterInput noLowerLetterUser = new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal3@yahoo.com", Password = "TEST1!" };

        //     RegisterInput noNumberUser = new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal4@yahoo.com", Password = "Testt!" };

        //     RegisterInput noNonAlphUser = new RegisterInput() { FirstName = "Password", LastName = "Validation", Roles = roles, Email = "PassVal5@yahoo.com", Password = "Test11" };

        //     RegisterInput[] badUsers = new RegisterInput[] {
        //         passwordTooShortUser,
        //         noUpperLetterUser,
        //         noLowerLetterUser,
        //         noNumberUser,
        //         noNonAlphUser
        //     };
        //     // iterate over the bad inputs and try to register each, assert that each returns a bad request
        //     foreach (RegisterInput user in badUsers)
        //     {
        //         var result = await controller.Register(user);
        //         // this will assert that the response returned a BadRequestObjectResult, and if it did it will cast our result (which is an IActionResult) to a BadRequestObjectResult
        //         BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //     }
        // }
    }
}
