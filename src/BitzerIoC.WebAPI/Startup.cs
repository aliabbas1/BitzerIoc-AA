using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Infrastructure.AppConstants;
using BitzerIoC.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace BitzerIoC.WebAPI
{
    public class Startup
    {
        public IHostingEnvironment _environemt;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _environemt = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                             .AllowAnyMethod()
                                                              .AllowAnyHeader()));

            services.AddMvcCore()
                  .AddJsonFormatters();
            //.AddAuthorization();

            services.AddTransient<IPasswordHasher<AspNetUser>, PasswordHasher<AspNetUser>>();

            //ToDo: Interface Injection
            services.AddScoped<IdentityContext>();
            //ToDo: Interface Injection
            services.AddScoped<IdentityRepository>();
            //ToDo: Interface Injection
            services.AddScoped<GatewayRepository>();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory
                .AddDebug()
                .AddConsole();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = GetAuthorityUri(_environemt),
                RequireHttpsMetadata = false,

                ScopeName = "BitzerIoC_WebAPI_Scope",
                ScopeSecret = "secret",
                AutomaticAuthenticate = true
            });

            app.UseCors("AllowAll");
            app.UseMvc();
         
        }

        /// <summary>
        /// Get the authority uri
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public string GetAuthorityUri(IHostingEnvironment environment)
        {
            return environment.IsDevelopment() ? UrlConstants.IdentityServerBaseUrlDevelopment : UrlConstants.IdentityServerBaseUrlProduction;
        }
    }
}
