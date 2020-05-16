using System;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Guests.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Http;
using Guests.Helpers;


using Guests.Models;
public class DatabaseFixture : IDisposable
{
    public AppUserContext DbContext { get; }
    public UserManager<AppUser> userManager { get; }
    public RoleManager<IdentityRole> roleManager { get; }
    public SignInManager<AppUser> signManager { get; }

    public AccountController accountController { get; }

    public IConfiguration Configuration { get; }
    public DatabaseFixture()
    {
        // get configuration
        Configuration = new ConfigurationBuilder().AddUserSecrets("321dfb55-5a08-441d-89a7-36cb8cba1e80").Build();
        // build a new connection string
        NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
        stringBuilder.Add("User ID", Configuration["TestUsername"]);
        stringBuilder.Add("Password", Configuration["TestPassword"]);
        stringBuilder.Add("Port", 5432);
        stringBuilder.Add("Database", Configuration["TestDatabase"]);
        stringBuilder.Add("Host", Configuration["TestHost"]);
        string connection = stringBuilder.ToString();

        // build a new dbcontext and create it
        var builder = new DbContextOptionsBuilder<AppUserContext>();
        builder.UseNpgsql(connection);
        DbContext = new AppUserContext(builder.Options);
        DbContext.Database.EnsureCreated();

        // We need Identity Managers for our controller constructor, we'll have to mock them up
        // we need user store for usermanager constructor
        var userStoreMock = new Mock<IUserStore<AppUser>>().Object;
        userManager = new Mock<UserManager<AppUser>>(userStoreMock, null, null, null, null, null, null, null, null).Object;

        // we need role store for rolemanager constructor
        var roleStoreMock = new Mock<IRoleStore<IdentityRole>>().Object;
        roleManager = new Mock<RoleManager<IdentityRole>>(roleStoreMock, null, null, null, null).Object;

        // we need context accessor and userPrincipalFactory for signManager
        var contextAccessor = new Mock<IHttpContextAccessor>().Object;
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object;
        signManager = new Mock<SignInManager<AppUser>>(userManager, contextAccessor, userPrincipalFactory, null, null, null, null).Object;

        // generate an account controller so we aren't making a new one for every test
        accountController = new AccountController(userManager, signManager, roleManager, DbContext);
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
    }
}
