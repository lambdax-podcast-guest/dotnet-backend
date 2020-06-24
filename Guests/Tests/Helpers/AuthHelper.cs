using System.Net.Http;
using System.Threading.Tasks;
using Guests.Tests.ReusableFixtures;

namespace Guests.Tests.Helpers
{
    public class AuthHelper
    {
        public class ResponseAsObject
        {
            public HttpResponseMessage Message { get; set; }
            public string Role { get; set; }

        }

        public class TestUser
        {
            public string Token { get; set; }
            public string Role { get; set; }

            public string Id { get; set; }
        }
        public static async Task<string> GenerateNonOwnerId(DatabaseFixture fixture)
        {
            HttpResponseMessage registerNonOwnerResponse = await AccountHelper.RegisterUniqueRegisterModel(fixture.httpClient);
            RegisterOutput registerNonOwnerOutput = await JsonHelper.TryDeserializeJson<RegisterOutput>(registerNonOwnerResponse);
            return registerNonOwnerOutput.id;
        }
    }
}
