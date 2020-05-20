
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace GuestTests
{
    public class AccountTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public AccountTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        /// <summary>Test that the register endpoint returns a token when the request is successful</summary>
        [Fact]
        public async void TestRegisterReturnsToken()
        {
            var controller = fixture.accountController;
            string[] roles = new string[] { "Guest" };

            RegisterInput guestUser = new RegisterInput() { FirstName = "Bob", LastName = "Ross", Roles = roles, Email = "BobRoss@yahoo.com", Password = "HappyLittleMistakes1!" };

            var result = await controller.Register(guestUser);

            // this will assert that the response returned a CreatedAtActionResult, and if it did it will cast our result (which is an IActionResult) to a CreatedAtActionResult
            CreatedAtActionResult okResult = Assert.IsType<CreatedAtActionResult>(result);

            // Assert that the response object has a token property
            bool hasToken = okResult.Value.GetType().GetProperty("token") != null;
            Assert.True(hasToken);
        }
    }
}
