using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BitzerIoC.Infrastructure.Routing;
using BitzerIoC.Infrastructure.AppConstants;

/// <summary>
/// BitzerIoC Startup file
/// </summary>
namespace BitzerIoC
{
    /// <summary>
    /// ToDo: Custom Error Page Middleware Pending Implementation (ErrorPageTest.sol project)
    /// ToDo: Exception Handler/Middleware pending 
    /// </summary>
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BitzerIoCConnectionString")));

            // Add framework services.
            services.AddMvc();

            services.AddSignalR(option =>
            {
                option.Hubs.EnableDetailedErrors = true;
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdminRoleRequired", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserRoleRequired", policy => policy.RequireClaim("role", RoleConstants.UserRole));
            });

            services.AddScoped<IdentityRepository>();
            //https://docs.asp.net/en/latest/fundamentals/dependency-injection.html
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Dashboard/Error");
            }

            app.UseStaticFiles();            

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true
            });

            #region OIDC Config - scopes
            var oidcOptions = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",
                RequireHttpsMetadata = false,
                ClientId = "BitzerIoC.Web",
                ClientSecret = "secret",
                ResponseType = "code id_token",
                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true
            };
            oidcOptions.Authority = env.IsDevelopment() ? UrlConstants.IdentityServerBaseUrlDevelopment : UrlConstants.IdentityServerBaseUrlProduction;
            oidcOptions.PostLogoutRedirectUri= env.IsDevelopment() ? UrlConstants.BitzerIoCBaseUrlDevelopment : UrlConstants.BitzerIoCBaseUrlProduction;
            oidcOptions.Scope.Clear();
            oidcOptions.Scope.Add("openid");
            oidcOptions.Scope.Add("profile");
            oidcOptions.Scope.Add("roles");
            oidcOptions.Scope.Add("email");
            oidcOptions.Scope.Add("BitzerIoC_WebAPI_Scope");
            app.UseOpenIdConnectAuthentication(oidcOptions);
            #endregion

            app.UseSignalR();
            MainRoutes.RegisterWebRoutes(app);
        }
    }
}
