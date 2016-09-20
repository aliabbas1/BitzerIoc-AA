using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Infrastructure.AppConstants
{
    public class UrlConstants
    {
       public static readonly string IdentityServerBaseUrlDevelopment = "http://localhost:5000";
       public static readonly string IdentityServerBaseUrlProduction = "http://local.identityserver.bitzerioc.com";

       public static readonly string  WebapiBaseUrlDevelopment = "http://localhost:5003/api";
       public static readonly string  WebapiBaseUrlProduction = "http://local.api.bitzerioc.com/api";

        public static readonly string BitzerIoCBaseUrlDevelopment = "http://localhost:5001";
        public static readonly string BitzerIoCBaseUrlProduction  = "http://local.bitzerioc.com";

        public static readonly string BitzerIoCAdminBaseUrlDevelopment = "http://localhost:5002";
        public static readonly string BitzerIoCAdminBaseUrlProduction  = "http://local.admin.bitzerioc.com";

    }
}
