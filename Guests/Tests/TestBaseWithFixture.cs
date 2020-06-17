using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;
namespace Guests.Tests
{
    // -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Base class that includes database fixture and xUnit outputter in the constructor
    /// Abstract class meant for inheritance only
    /// 
    /// All classes that inherit from this class will share the SAME database and server instance
    /// </summary>
    // -------------------------------------------------------------------------------------------------
    [Collection("DbCollection")]
    public abstract class TestBaseWithFixture
    {
        public DatabaseFixture fixture { get; private set; }
        public ITestOutputHelper outputter { get; private set; }

        public TestBaseWithFixture(DatabaseFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            // Use output.WriteLine to print to console
            // This ITestOutputHelper class only knows how to use the Visual Studio Output though, so to tell it to use the console here in VSCode, run the test command like this:
            // dotnet test -l "console;verbosity=detailed"
            outputter = output;
        }
        /// <summary>
        /// Override this method on the derived class with [MemberData] or [ClassData] to test your endpoints that use the Authorize attribute
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        public virtual async Task AuthorizeAttributeTests(string endpoint, HttpMethod method)
        {
            // create a new request message
            HttpRequestMessage requestMessageNoHeaders = new HttpRequestMessage(method, endpoint);

            // send the request with no headers
            HttpResponseMessage responseWithoutAuthHeaders = await fixture.httpClient.SendAsync(requestMessageNoHeaders);

            // register and login a unique GUEST 
            HttpResponseMessage loginResponse = await AccountHelper.RegisterAndLogInNewUser(fixture.httpClient, new string[] { "Guest" });

            // deserialize the response and get the token
            LoginOutput loginOutput = await JsonHelper.TryDeserializeJson<LoginOutput>(loginResponse);
            string token = loginOutput.token;

            // generate a new request with headers this time
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, endpoint);
            AuthenticationHeaderValue authHeader;
            bool isValidHeader = AuthenticationHeaderValue.TryParse($"Bearer {token}", out authHeader);
            requestMessage.Headers.Authorization = authHeader;
            HttpResponseMessage responseWithAuthHeaders = await fixture.httpClient.SendAsync(requestMessage);

            using(new AssertionScope())
            {
                responseWithoutAuthHeaders.IsSuccessStatusCode.Should().BeFalse($"because we expect the {method.ToString()} request to the {endpoint} to fail without auth headers");

                responseWithAuthHeaders.IsSuccessStatusCode.Should().BeTrue($"because we expect the {method.ToString()} request to the {endpoint} to succeed with a valid token on the request headers");
            }
        }

    }
}
