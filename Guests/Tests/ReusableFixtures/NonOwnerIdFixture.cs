using System.Threading.Tasks;
using Xunit;
namespace Guests.Tests
{
    /// <summary>
    /// Inherit from this class to be able to reuse a generic user id as a parameter on user owned resources
    /// </summary>
    public class NonOwnerIdFixture : IAsyncLifetime
    {
        public NonOwnerIdFixture(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        public async Task InitializeAsync()
        {
            nonOwnerId = await AuthHelper.GenerateNonOwnerId(_fixture);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
        public string nonOwnerId { get; set; }
        public DatabaseFixture _fixture;
    }
}
