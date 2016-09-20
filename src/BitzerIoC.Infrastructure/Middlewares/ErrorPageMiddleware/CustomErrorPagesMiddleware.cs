using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Infrastructure.Middlewares.ErrorPageMiddleware
{
    /// <summary>
    /// ToDo: CustomErrorPage Middleware 
    /// How to call from web project :: ErrorPageTest.sol
    /// app.UseCustomErrorPages(ErrorPages as Dictionary<int,string>);
    /// </summary>

    public class CustomErrorPagesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IDictionary<int, string> _errorPages;

        public CustomErrorPagesMiddleware(ILoggerFactory loggerFactory, RequestDelegate next, IDictionary<int, string> errorPages)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CustomErrorPagesMiddleware>();
            _errorPages = errorPages;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "An unhandled exception has occurred while executing the request");

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error page middleware will not be executed.");
                    throw;
                }
                try
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(0, ex2, "An exception was thrown attempting to display the error page.");
                }
                throw;
            }
            finally
            {
                var statusCode = context.Response.StatusCode;

                if (_errorPages.Keys.Contains(statusCode))
                {
                    context.Request.Path = _errorPages[statusCode];
                    await _next(context);
                }
            }
        }
    }

    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseCustomErrorPages(this IApplicationBuilder app, Dictionary<int, string> errorPages)
        {
            return app.UseMiddleware<CustomErrorPagesMiddleware>(errorPages);
        }
    }
}
