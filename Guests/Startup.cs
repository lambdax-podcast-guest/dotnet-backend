using System;
using System.Threading.Tasks;
using Guests.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Guests
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
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
            // services.AddControllers();
            services.AddDbContext<GuestsContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async (context) => {
                // Sanity check the Server
                await context.Response.WriteAsync("Server is up!!!");
            });
        }
    }

}
