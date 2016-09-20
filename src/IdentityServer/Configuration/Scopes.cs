using IdentityServer4.Models;
using System.Collections.Generic;

namespace Host.Configuration
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.RolesAlwaysInclude,

                new Scope
                {
                    Name = "BitzerIoC_WebAPI_Scope",
                    DisplayName = "BitzerIoC WebAPI for Services",
                    Description = "Provide services to the authenticated applications.",
                    //Resource like API
                    Type = ScopeType.Resource,
                    ScopeSecrets = new List<Secret>
                    {
                        new Secret("lodam-electronics".Sha256())
                    },
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },

                #region Testing Purpose
                new Scope
                    {
                        Name = "customscope",
                        DisplayName = "Custom Scope",
                        Type = ScopeType.Identity,
                        Emphasize = false,
                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim("customproperty", true)
                        }
                    },
                   new Scope
                    {
                        Name = "customscopewithsecretkey",
                        DisplayName = "Custom Secret Scope",
                        Type = ScopeType.Identity,
                        Emphasize = false,
                         ScopeSecrets = new List<Secret>
                            {
                                new Secret("secret".Sha256())
                            },
                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim("role", true)
                        }
                    }
                   #endregion

            };
        }
    }
}