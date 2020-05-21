
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace GuestTests
{
    public class AccountTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public AccountTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        // -------------------------------------------------------------------------------------------------
        /// <summary>Test that the register endpoint returns a token when the request is successful</summary>
        // -------------------------------------------------------------------------------------------------
        [Fact]
        public async void TestRegisterReturnsToken()
        {
            // var controller = fixture.accountController;
            string[] roles = new string[] { "Guest" };

            RegisterInput guestUser = new RegisterInput() { FirstName = "Bob", LastName = "Ross", Roles = roles, Email = "BobRoss@yahoo.com", Password = "HappyLittleMistakes1!" };

            var content = JsonContent.Create(guestUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



            var result = await fixture.httpClient.PostAsync("/api/account/register", content);

            bool isSuccessful = result.IsSuccessStatusCode;
            Assert.True(isSuccessful);
            // this will assert that the response returned a CreatedAtActionResult, and if it did it will cast our result (which is an IActionResult) to a CreatedAtActionResult
            // CreatedAtActionResult okResult = Assert.IsType<CreatedAtActionResult>(result.Content);

            // // Assert that the response object has a token property
            bool hasToken = result.Content.GetType().GetProperty("token") != null;
            Assert.True(hasToken);
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
