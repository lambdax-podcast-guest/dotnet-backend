
using System;
using Xunit;
using Guests.Helpers;
using Guests.Models.Inputs;
using Microsoft.AspNetCore.Mvc;

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

            // get the Task<IActionResult>
            var result = await controller.Register(guestUser);
            // cast it to an ok result in order to get the value
            // It is returning bad request right now, so I am casting it to badrequest instead, something wrong with the role/rolemanager
            BadRequestObjectResult okResult = result as BadRequestObjectResult;


            // Assert.Equal(2, okResult.StatusCode);
            Assert.Equal("something that will make it fail so we can see the output", okResult.Value);
            // once it is working this should work
            // var hasToken = okResult.Value.token != null;

            // Assert.True(hasToken);
        }
    }
}
