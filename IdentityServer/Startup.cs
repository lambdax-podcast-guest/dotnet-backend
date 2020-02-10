// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using DotNetEnv;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        private static string secret => DotNetEnv.Env.GetString("SECRET");


        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            // services.AddControllersWithViews();

            var builder = services.AddIdentityServer()
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients);

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            JwtSecurityTokenHandler.DefaultMapInboundClaims=false;

            services.AddAuthentication(opts=>{
                opts.DefaultScheme="Cookies";
                opts.DefaultChallengeScheme="oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc",opts=>{
                opts.Authority="https://localhost:5000";
                opts.RequireHttpsMetadata=false;
                opts.ClientId="mvc";
                opts.ClientSecret=secret;
                opts.ResponseType="code";

                opts.SaveTokens=true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            // app.UseStaticFiles();
            // app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            // app.UseAuthorization();
            // app.UseEndpoints(endpoints =>
            // {
            //    endpoints.MapDefaultControllerRoute();
            // });
        }
    }
}
