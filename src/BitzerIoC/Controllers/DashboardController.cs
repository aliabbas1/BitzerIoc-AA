using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Infrastructure.Filters;
using BitzerIoC.Infrastructure.AppConstants;
using BitzerIoC.Infrastructure.Utilities;
using System.Security.Claims;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using BitzerIoC.Domain.ViewModels;

namespace BitzerIoC.Controllers
{

    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            _logger.LogError("Error :: An error occured in Dashboard");
            return View();
        }


        public async Task<IActionResult> Test()
        {
            //http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client

            //using (HttpClient client = new HttpClient())
            //{
            //    var requestUri = ""; 
            //    client.BaseAddress = new Uri("http://localhost:5003/");
            //    requestUri = "/api/GatewayOperations";
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.PostAsJsonAsync(requestUri, new GatewayViewModel() { GatewayId=1, GatewayName="Gateway 1", Status= false, UpdatedBy="Admin" });
            //}

            return View();
        }

    }
}
