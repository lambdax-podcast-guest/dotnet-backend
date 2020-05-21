using System;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Guests.Helpers;
using Guests.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Linq;
using Npgsql;

namespace Guests
{
    public class Startup
    {
        private string _connection = null;
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            environment = hostEnvironment;
        }

        // internal and static for dependency injection within any child
        internal static IConfiguration Configuration { get; private set; }
        internal static IWebHostEnvironment environment { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Custom",
               builder => builder.WithOrigins(Configuration["CorsOrigin"].Split(' ')).AllowAnyHeader().AllowAnyMethod()));
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            if (environment.IsDevelopment())
            {
                // This is how you add in the secrets to the connectionString 
                // Append the secrets to the end of the string

                builder.Add("User ID", Configuration["HerokuUsername"]);
                builder.Add("Password", Configuration["HerokuPassword"]);
                builder.Add("Port", 5432);
                builder.Add("Database", Configuration["HerokuDatabase"]);
                builder.Add("Host", Configuration["HerokuHost"]);
            }
            else
            {
                Uri dbUrl = new Uri(Configuration["DATABASE_URL"]);
                string[] userInfo = dbUrl.UserInfo.Split(':');
                builder.Add("User ID", userInfo[0]);
                builder.Add("Password", userInfo[1]);
                builder.Add("Host", dbUrl.Host);
                builder.Add("Port", dbUrl.Port);
                builder.Add("Database", dbUrl.LocalPath.TrimStart('/'));
            }

            // make the null value of _connection equal the newly built connectionString
            _connection = builder.ToString();

            services.AddMvc();

            // add swashBuckle through swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Podcast Guests API",
                    Version = "v1",
                    Description = "Backend for Podcast Guests",
                    // TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Charlie FN Rogers, Steve Smodish, Brandon Porter, David Freitag",
                        Url = new Uri("https://lambdax-podcast-guest.github.io/FrontEndView/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Using MIT Open Source License",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
            }
            );

            // allow us to use HttpContext within DbContext
            services.AddHttpContextAccessor();

            // Connect to the DB 
            services.AddDbContext<AppUserContext>(options => options.UseNpgsql(_connection).UseSnakeCaseNamingConvention());

            // Add Identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppUserContext>()
                .AddDefaultTokenProviders();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> _roleManager)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // Enable Cors
            app.UseCors("Custom");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DataInitializer.SeedData(_roleManager);

        }
    }

}