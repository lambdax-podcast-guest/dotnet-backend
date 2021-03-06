using System;
using System.Net.Http;
using Guests.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit;

namespace Guests.Tests
{

    /// <summary>
    /// Database fixture to be used in the Database Collection decorator to be shared among all the test classes
    /// Contains instance of AppUserContext DbContext, IConfiguration Configuration, HttpClient httpClient and TestServer testServer
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        public AppUserContext DbContext { get; }

        public IConfiguration Configuration { get; }

        public HttpClient httpClient { get; }

        public TestServer testServer { get; }

        public DatabaseFixture()
        {
            // get configuration in order to create our connection string
            Configuration = new ConfigurationBuilder().AddUserSecrets("321dfb55-5a08-441d-89a7-36cb8cba1e80").Build();

            // we need to create the database in order for our startup to run, just like running our migrations
            // create the connection string
            NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
            stringBuilder.Add("User ID", Configuration["TestUsername"]);
            stringBuilder.Add("Password", Configuration["TestPassword"]);
            stringBuilder.Add("Port", 5432);
            stringBuilder.Add("Database", Configuration["TestDatabase"]);
            stringBuilder.Add("Host", Configuration["TestHost"]);
            string connection = stringBuilder.ToString();

            // build the connection options
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<AppUserContext>();
            optionsBuilder.UseNpgsql(connection).UseSnakeCaseNamingConvention();

            // so our AppUserContext is expecting type DbContextOptions<AppUserContext> but DbContextOptionsBuilder.Options returns type DbContextOptions, so we have to explicitely cast to DbContextOptions<AppUserContext>
            var options = optionsBuilder.Options as DbContextOptions<AppUserContext>;

            DbContext = new AppUserContext(options);
            DbContext.Database.EnsureCreated();

            // now we need to start up a new test server, which takes in a WebHostBuilder
            WebHostBuilder webHostBuilder = new WebHostBuilder();

            // make sure to use the test environment when we use startup
            webHostBuilder.UseConfiguration(Configuration).UseEnvironment("Test").UseStartup<Startup>();

            // create the test server
            testServer = new TestServer(webHostBuilder);

            // create the http client
            httpClient = testServer.CreateClient();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            testServer.Dispose();
        }
    }

    [CollectionDefinition("DbCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
