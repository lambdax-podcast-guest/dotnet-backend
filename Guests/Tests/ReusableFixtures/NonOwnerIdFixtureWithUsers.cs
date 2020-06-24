using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Guests.Tests
{
    /// <summary>
    /// Inherit from this class to be able to reuse a generic user id as a parameter on user owned resources
    /// </summary>
    public class NonOwnerIdFixtureWithUsers : IAsyncLifetime
    {
        public NonOwnerIdFixtureWithUsers(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        public async Task InitializeAsync()
        {
            // generate a unique user for a non owner id
            nonOwnerId = await AuthHelper.GenerateNonOwnerId(_fixture);

            // for each role we will save a TestUser object that contains that user's id, token and role to be used to test endpoints that call for a user id
            testUsers = new Dictionary<string, AuthHelper.TestUser>();

            // all our different roles are stored in the role entity, iterate over them
            FieldInfo[] roles = Type.GetType("Guests.Entities.Role").GetFields().ToArray();

            foreach (FieldInfo role in roles)
            {
                // register a user to get their id and token
                HttpResponseMessage message = await AccountHelper.RegisterUniqueRegisterModel(_fixture.httpClient, new string[] { role.Name });
                // deserialize
                RegisterOutput registerOutput = await JsonHelper.TryDeserializeJson<RegisterOutput>(message);

                testUsers.Add(role.Name, new AuthHelper.TestUser() { Id = registerOutput.id, Role = role.Name, Token = registerOutput.token });
            }
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// nonOwnerId to be used to test endpoints where the user requesting the resource is not the owner of the resource
        /// </summary>
        public string nonOwnerId { get; set; }

        /// <summary>
        /// Dictionary with roles as keys that provides user info including token, id and role that can be used to make requests to either endpoints they own, or endpoints they do not own
        /// </summary>
        public Dictionary<string, AuthHelper.TestUser> testUsers { get; set; }
        public DatabaseFixture _fixture;

    }
}
