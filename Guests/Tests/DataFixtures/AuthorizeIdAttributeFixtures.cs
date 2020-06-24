using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Guests.Entities;
using Guests.Models.Inputs;
namespace Guests.Tests.DataFixtures
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

    /// <summary>
    /// IEnumerable to be used to test that the AuthorizeId attribute allows the provided roles to access a resource
    /// </summary>
    public class EndpointsAndMethodsForRolesTestOnAuthorizeId : IEnumerable<object[]>
    {
        // Get Enumerator returns our list of endpoints and methods
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "/api/guests", HttpMethod.Get, new string[] { Role.Guest, Role.Host }, null };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
