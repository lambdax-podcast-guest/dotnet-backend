using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Guests.Models.Inputs;
namespace Guests.Tests
{
    /// <summary>
    /// IEnumerable to be used to test that the AuthorizeId attribute rejects a request with no headers
    /// </summary>
    public class EndpointsAndMethodsForNoHeadersTestsOnAuthorizeId : IEnumerable<object[]>
    {
        // Get Enumerator returns our list of endpoints and methods
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "/api/guests", HttpMethod.Get, null };
            yield return new object[] { "/api/account", HttpMethod.Delete, null };
            yield return new object[] { "/api/account", HttpMethod.Put, new UpdateUser() { FirstName = "AuthorizeIdUpdatedFirstName", LastName = "AuthorizeIdUpdatedLastName" } };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
