using System.Net.Http;
using System.Net.Http.Headers;
using FluentAssertions;
using FluentAssertions.Execution;
using Guests.Tests.Helpers;
using Guests.Tests.ReusableFixtures;
using Xunit;
using Xunit.Abstractions;

namespace Guests.Tests
{
    public class GuestTests : TestBaseWithFixture
    {
        public GuestTests(DatabaseFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        /// <summary>
        /// Test that the get guests endpoint does not return the guests password hash on the response body object
        /// </summary>
        [Fact]
        public async void TestGetGuestsDoesNotReturnPasswordHash()
        {
            // register and login a unique GUEST 
            HttpResponseMessage loginResponse = await AccountHelper.RegisterAndLogInNewUser(fixture.httpClient, new string[] { "Guest" });

            // deserialize the response and get the token
            LoginOutput loginOutput = await JsonHelper.TryDeserializeJson<LoginOutput>(loginResponse);
            string token = loginOutput.token;

            // attempt to set the authorization field of the request headers
            AuthenticationHeaderValue authHeader;
            bool isValidHeader = AuthenticationHeaderValue.TryParse($"Bearer {token}", out authHeader);

            // if the header isn't valid fail the test so there aren't a bunch of errors when we run the request
            isValidHeader.Should().BeTrue("because we expect the token we got from the login response to be a valid string, and for that string to be an acceptable authorization header");

            fixture.httpClient.DefaultRequestHeaders.Authorization = authHeader;
            // make a get request to /api/guests
            // there will probably be other guests in there from other tests we have run
            // but since any other data in the db is not dependable, and the only thing we care about in this test is that each guest object does not have the password hash field, we will simply test the first element in the array
            HttpResponseMessage getGuestsResponse = await fixture.httpClient.GetAsync("/api/guests");

            GuestOutput[] guests = await JsonHelper.TryDeserializeJson<GuestOutput[]>(getGuestsResponse);

            // test that the first element of the array does not have the password hash field in any casing
            using(new AssertionScope())
            {
                GuestOutput guest = guests[0];
                string message = "because the guest object should not contain a password hash field in any casing";
                guest.password_hash.Should().BeNull(message);
                guest.passwordHash.Should().BeNull(message);
                guest.Password_Hash.Should().BeNull(message);
                guest.PasswordHash.Should().BeNull(message);
            }
        }
    }
}
