// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using DotNetEnv;

namespace IdentityServer
{
    public static class Config
    {
        static Config(){
            DotNetEnv.Env.Load();
        }
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>{
                new ApiResource("guests","Guests")
            };
        
        public static IEnumerable<Client> Clients =>
            new List<Client>{
                new Client{
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets={
                        new Secret(secret.Sha256())
                    },
                    AllowedScopes={
                        "guests",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RequireConsent=false,
                    RequirePkce=true,
                    AllowOfflineAccess=true,
                    
                }
            };

        

        private static string secret => DotNetEnv.Env.GetString("SECRET");
    }

}