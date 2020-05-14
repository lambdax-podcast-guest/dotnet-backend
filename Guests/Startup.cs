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

namespace Guests
{
    public class Startup
    {
        private string _connection = null;
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // internal and static for dependency injection within any child
        internal static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll",
               builder => builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod()));
            // This is how you add in the secrets to the connectionString 
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
            // Append the secrets to the end of the string
            builder.Add("User ID", Configuration["HerokuUsername"]);
            builder.Add("Password", Configuration["HerokuPassword"]);
            builder.Add("Host", Configuration["HerokuHost"]);
            builder.Add("Post", Configuration["HerokuPost"]);
            builder.Add("Database", Configuration["HerokuDatabase"]);
            builder.Add("Pooling", "true");
            builder.Add("SSL Mode", "Require");
            builder.Add("TrustServerCertificate", "True");

            // make the null value of _connection equal the newly built connectionString
            _connection = builder.ConnectionString;

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
            if (env.IsDevelopment())
            {
                app.UseCors("AllowAll");
                app.UseDeveloperExceptionPage();
            }

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