using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.Interfaces;
using BitzerIoC.Infrastructure.Repositories;
using Host.Configuration;
using Host.Services;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Host
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IdentityContext>();
            services.AddScoped<IIdentityRepository,IdentityRepository>();

            var cert = new X509Certificate2(Path.Combine(_environment.ContentRootPath, "idsvr3test.pfx"), "idsrv3test");

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteractionOptions.LoginUrl = "/ui/login";
                options.UserInteractionOptions.LogoutUrl = "/ui/logout";
                options.UserInteractionOptions.ConsentUrl = "/ui/consent";
                options.UserInteractionOptions.ErrorUrl = "/ui/error";
            })
            .SetSigningCredential(cert)
            .AddInMemoryScopes(Scopes.Get())
            .AddInMemoryClients(Clients.Get(_environment));
                   
            builder.Services.AddTransient<IProfileService, ProfileService>();
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            builder.Services.AddTransient<IPasswordHasher<AspNetUser>, PasswordHasher<AspNetUser>>();
            builder.AddCustomGrantValidator<Extensions.CustomGrantValidator>();

            // for the UI
            services
                .AddMvc()
                .AddRazorOptions(razor =>
                {
                    razor.ViewLocationExpanders.Add(new UI.CustomViewLocationExpander());
                });
            services.AddTransient<UI.Login.LoginService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            Func<string, LogLevel, bool> filter = (scope, level) =>
                scope.StartsWith("IdentityServer") ||
                scope.StartsWith("IdentityModel") ||
                level == LogLevel.Error ||
                level == LogLevel.Critical;

            loggerFactory.AddConsole(filter);
            loggerFactory.AddDebug(filter);

            app.UseDeveloperExceptionPage();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Temp",
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}