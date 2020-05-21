using System;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Guests.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Guests.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Guests;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;



public class DatabaseFixture : IDisposable
{
    public AppUserContext DbContext { get; }
    // public UserManager<AppUser> userManager { get; }
    // public RoleManager<IdentityRole> roleManager { get; }
    // public SignInManager<AppUser> signManager { get; }

    // public AccountController accountController { get; }

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



        // build a new connection string
        // NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
        // stringBuilder.Add("User ID", Configuration["TestUsername"]);
        // stringBuilder.Add("Password", Configuration["TestPassword"]);
        // stringBuilder.Add("Port", 5432);
        // stringBuilder.Add("Database", Configuration["TestDatabase"]);
        // stringBuilder.Add("Host", Configuration["TestHost"]);
        // string connection = stringBuilder.ToString();

        // // Build service collection to create identity UserManager and RoleManager. 
        // IServiceCollection serviceCollection = new ServiceCollection();

        // // build a new dbcontext and create the database
        // serviceCollection.AddDbContext<AppUserContext>(options => options.UseNpgsql(connection));
        // serviceCollection.AddLogging();
        // DbContext = serviceCollection.BuildServiceProvider().GetService<AppUserContext>();
        // DbContext.Database.EnsureCreated();

        // // Add Identity to create UserManager and RoleManager.
        // serviceCollection.AddIdentity<AppUser, IdentityRole>(options =>
        //     {
        //         options.User.RequireUniqueEmail = true;
        //     })
        //         .AddRoles<IdentityRole>()
        //         .AddEntityFrameworkStores<AppUserContext>()
        //         .AddDefaultTokenProviders();


        // // add our jwt auth
        // serviceCollection
        //         .AddAuthentication(options =>
        //         {
        //             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //         })
        //         .AddJwtBearer(options =>
        //         {
        //             options.RequireHttpsMetadata = false;
        //             options.SaveToken = true;
        //             options.TokenValidationParameters = new TokenValidationParameters
        //             {
        //                 ValidIssuer = Configuration["Guests:JwtIssuer"],
        //                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Guests:JwtKey"])),
        //                 ValidateAudience = false
        //             };
        //         });
        // // create UserManager
        // userManager = serviceCollection.BuildServiceProvider().GetService<UserManager<AppUser>>();
        // // create roleManager
        // roleManager = serviceCollection.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
        // // create signManager
        // signManager = serviceCollection.BuildServiceProvider().GetService<SignInManager<AppUser>>();
        // // SignManager needs an HTTP context, but to create one we need to build our service provider
        // ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        // // create a default http context and add it to the signManager context property
        // signManager.Context = new DefaultHttpContext { RequestServices = serviceProvider };
        // // generate an account controller so we aren't making a new one for every test
        // accountController = new AccountController(userManager, signManager, roleManager, DbContext, Configuration);
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        testServer.Dispose();
    }
}
