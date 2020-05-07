using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Text;
using Guests.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace Guests
{
    public class Startup
    {
        private string _connection = null;
        public Startup(IConfiguration configuration) => Configuration = configuration;
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
                options.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Podcast Guests API", 
                    Version = "v1",
                    Description = "An example of an ASP.NET Core Web API",
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
                })
            );

            // Connect to the DB 
            services.AddDbContext<GuestsContext>(options =>
            {
                options.UseNpgsql(_connection);
            });

            // Add Identity
            services.AddIdentity<AppUser, IdentityRole<string>>()
                .AddEntityFrameworkStores<GuestsContext>()
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
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) => {
                // Sanity check the Server
                await context.Response.WriteAsync("Server is up, let's rock");
            });
        }
    }

}
