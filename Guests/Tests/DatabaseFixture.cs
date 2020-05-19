using System;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Guests.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.RegularExpressions;
using Guests.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;



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

        // Build service collection to create identity UserManager and RoleManager. 
        IServiceCollection serviceCollection = new ServiceCollection();

        // build a new dbcontext and create the database
        serviceCollection.AddDbContext<AppUserContext>(options => options.UseNpgsql(connection));
        serviceCollection.AddLogging();
        DbContext = serviceCollection.BuildServiceProvider().GetService<AppUserContext>();
        DbContext.Database.EnsureCreated();

        // Add Identity to create UserManager and RoleManager.
        serviceCollection.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppUserContext>()
                .AddDefaultTokenProviders();

        // add our jwt auth
        serviceCollection
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Guests:JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Guests:JwtKey"])),
                        ValidateAudience = false
                    };
                });
        // create UserManager
        userManager = serviceCollection.BuildServiceProvider().GetService<UserManager<AppUser>>();
        // create roleManager
        roleManager = serviceCollection.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
        // create signManager
        signManager = serviceCollection.BuildServiceProvider().GetService<SignInManager<AppUser>>();
        // SignManager needs an HTTP context, but to create one we need to build our service provider
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        // create a default http context and add it to the signManager context property
        signManager.Context = new DefaultHttpContext { RequestServices = serviceProvider };
        // generate an account controller so we aren't making a new one for every test
        accountController = new AccountController(userManager, signManager, roleManager, DbContext, Configuration);
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
    }
}
