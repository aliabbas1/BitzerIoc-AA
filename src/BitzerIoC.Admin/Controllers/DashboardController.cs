using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Infrastructure.AppConstants;
using BitzerIoC.Infrastructure.Utilities;
using BitzerIoC.Infrastructure.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using BitzerIoC.Domain.ViewModels;
using BitzerIoC.Domain.Interfaces;
using BitzerIoC.Domain.Entities;

namespace BitzerIoC.Admin.Controllers
{
   
    public class DashboardController : Controller
    {

        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
           _logger= logger;
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult Devices()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult Gateways()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult CreateGateway()
        {
            return View();
        }

 

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult UpdateUser()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public ActionResult Users()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public ActionResult UpdateGateway()
        {
            return View(new GatewayViewModel());
        }

        [HttpGet]
        [Authorize]
        [AdminAccessFilter]
        public IActionResult Profile()
        {
            return View();
        }

       

        public IActionResult Error()
        {
            _logger.LogError("Error :: An error occured in Dashboard");
            return View();
        }

    }
}
