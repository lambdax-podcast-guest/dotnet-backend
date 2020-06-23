using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

namespace Guests.Tests
{

    /// <summary>
    /// IEnumerable to be used in the AuthorizeAttribute tests
    /// </summary>
    public class EndpointsAndMethodsForAuthorize : IEnumerable<object[]>
    {
        // Get Enumerator returns our list of endpoints and methods
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "/api/guests", HttpMethod.Get };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
