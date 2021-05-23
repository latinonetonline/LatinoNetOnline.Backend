// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;

using LatinoNetOnline.Backend.Modules.Identities.Web.Options;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Identities.Web
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };


        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var identityOptions = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();

            return new Client[]
            {
                new Client
                {
                    ClientId = "LatinoNetOnline.m2m",
                    ClientSecrets = { new Secret(identityOptions.ClientSecret.Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.LocalApi.ScopeName
                    }
                },
            };
        }
    }
}