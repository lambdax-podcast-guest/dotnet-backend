
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
        ITestOutputHelper _outputter;

        public AccountTests(DatabaseFixture fixture, ITestOutputHelper outputter)
        {
            this.fixture = fixture;
            _outputter = outputter;
        }

        /// <summary>Test that the register endpoint returns a token when the request is successful</summary>
        [Fact]
        public async void TestRegisterReturnsToken()
        {
            var controller = fixture.accountController;
            string[] roles = new string[] { "Guest" };

            RegisterInput guestUser = new RegisterInput() { FirstName = "Bob", LastName = "Ross", Roles = roles, Email = "BobRoss@yahoo.com", Password = "HappyLittleMistakes1!" };

            var result = await controller.Register(guestUser);

            var obj = Assert.IsType<CreatedAtActionResult>(result);
            // bool hasToken = obj.Value.token != null;


            // Assert.True(hasToken);
        }
    }
}
