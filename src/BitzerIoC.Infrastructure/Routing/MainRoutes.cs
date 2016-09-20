using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace BitzerIoC.Infrastructure.Routing
{
    public class MainRoutes
    {
        /// <summary>
        /// BitzerIoC Application routes
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        public static void RegisterWebRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Dashboard}/{action=Index}/{id?}");
            });

            app.UseMvc(routes =>
            {
                //ToDo: Need to implement SPA Dashboard,View in BitzerIoC
                #region SPA Routes
                routes.MapRoute(name: "BitzerIoC-SPA",
                                   template: "main/{page}/{*filters}",
                                   defaults: new { controller = "Dashboard", action = "Index" });
                #endregion

            });
        }


        /// <summary>
        /// BitzerIoC.Admin Application Routes
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterAdminRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Dashboard}/{action=Index}/{id?}");
            });

            app.UseMvc(routes =>
            {
               
                #region Admin Module SPA Routes
                routes.MapRoute(name: "Admin-Module-SPA",
                                   template: "admin/{page}/{*filters}",
                                   defaults: new { controller = "Dashboard", action = "Index" });
                #endregion

             });
        }


    }
}
