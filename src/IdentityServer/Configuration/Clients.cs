using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;

namespace Host.Configuration
{
    public static class Clients
    {
 
        public static IEnumerable<Client> Get(IHostingEnvironment environment)
        {
            List<Client> clients = new List<Client>();

            #region BitzerIoC Client (BitzerIoC.Web)
            Client webClient = new Client
            {
                RequireConsent = false,
                ClientId = "BitzerIoC.Web",
                ClientName = "BitzerIoC User Portal",
                AllowedGrantTypes = GrantTypes.Hybrid,
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },

                AllowedScopes = new List<string>
                    {
                        //Client make request to grant access to the claims of user like email etc.Same as we have application which access google
                        //and we need some user information
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,
                       "BitzerIoC_WebAPI_Scope"
                    }
            };

            webClient.RedirectUris = GetClientRedirectUris(webClient.ClientId,environment);
            webClient.PostLogoutRedirectUris = GetClientPostRedirectUris(webClient.ClientId, environment);
            webClient.AllowedCorsOrigins = GetClientCORSUris(webClient.ClientId, environment);
            clients.Add(webClient);
            #endregion

            #region Admin Client (BitzerIoC.Admin)
            Client adminClient = new Client
            {
                RequireConsent = false,
                ClientId = "BitzerIoC.Admin",
                ClientName = "BitzerIoC Admin Portal",
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Hybrid,
                AllowedScopes = new List<string>
                    {
                       //Scope contains user claims
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,
                        "BitzerIoC_WebAPI_Scope"
                    }
            };
            adminClient.RedirectUris = GetClientRedirectUris(adminClient.ClientId, environment);
            adminClient.PostLogoutRedirectUris = GetClientPostRedirectUris(adminClient.ClientId, environment);
            adminClient.AllowedCorsOrigins = GetClientCORSUris(adminClient.ClientId, environment);
            clients.Add(adminClient);
            #endregion

            
            Client jsClient = new Client
            {
                RequireConsent = false,
                ClientId = "BitzerIoC_JS_APP",
                ClientName = "BitzerIoC Javascript Application",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,

                        "BitzerIoC_WebAPI_Scope","customscope","customscopewithsecretkey"
                    }
            };

            jsClient.RedirectUris = GetClientRedirectUris(jsClient.ClientId, environment);
            jsClient.AllowedCorsOrigins = GetClientCORSUris(jsClient.ClientId, environment);
            clients.Add(jsClient);

            return clients;
        }




        /// <summary>
        /// Return RedirectUris based on environment
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="environment"></param>
        /// <returns></returns>
        private static List<string> GetClientRedirectUris(string clientId,IHostingEnvironment environment)
        {
            switch(clientId)
            {
                case "BitzerIoC.Web":
                {
                          if (environment.IsDevelopment())
                           {
                                    return new List<string>
                            {
                              "http://localhost:5001/signin-oidc"
                            };
                                }
                                if (environment.IsProduction())
                                {
                                    return new List<string>
                            {
                              "http://local.bitzerioc.com/signin-oidc"
                            };
                         }
                  break;
                 }

                case "BitzerIoC.Admin":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                              "http://localhost:5002/signin-oidc"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                              "http://local.admin.bitzerioc.com/signin-oidc"
                            };
                        }
                        break;
                    }
                case "BitzerIoC_JS_APP":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                              "http://localhost:5001/Home/JavascriptClient"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                              "http://local.bitzerioc.com/Home/JavascriptClient"
                            };
                        }
                        break;
                    }
             }

            return null;
        }
        /// <summary>
        /// Retrun PostLogoutUri based on environment
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="environment">Hosting Environemnt object</param>
        /// <returns></returns>
        private static List<string> GetClientPostRedirectUris(string clientId, IHostingEnvironment environment)
        {
            switch (clientId)
            {
                case "BitzerIoC.Web":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                              "http://localhost:5001"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                              "http://local.bitzerioc.com"
                            };
                        }
                        break;
                    }

                case "BitzerIoC.Admin":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                              "http://localhost:5002"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                              "http://local.admin.bitzerioc.com"
                            };
                        }
                        break;
                    }
             }

            return null;
        }
        /// <summary>
        /// Return Cross Orgin Policy URIS on the base of environemt
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        private static List<string> GetClientCORSUris(string clientId, IHostingEnvironment environment)
        {
            switch (clientId)
            {
                case "BitzerIoC.Web":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                               "http://localhost:5001"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                               "http://local.bitzerioc.com"
                            };
                        }
                        break;
                    }

                case "BitzerIoC.Admin":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                             "http://localhost:5002"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                              "http://local.admin.bitzerioc.com"
                            };
                        }
                        break;
                    }
                case "BitzerIoC_JS_APP":
                    {
                        if (environment.IsDevelopment())
                        {
                            return new List<string>
                            {
                               "http://localhost:5001", "http://localhost:5002"
                            };
                        }
                        if (environment.IsProduction())
                        {
                            return new List<string>
                            {
                              "http://local.bitzerioc.com", "http://local.admin.bitzerioc.com"
                            };
                        }
                        break;
                    }
            }

            return null;
        }

    }
}