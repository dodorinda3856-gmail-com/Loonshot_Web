using LoonshotTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using LoonshotTest.Models.Login;
using LoonshotTest.Filters;
using Microsoft.AspNetCore.SignalR;
using LoonshotTest.Hubs;
using LoonshotTest.Interface;
using Microsoft.AspNetCore.Http;

namespace LoonshotTest.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        
        public IActionResult Index()
        {
            Log.Infomation("HomeController-Index-27", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
            return View();
        }

        private IActionResult myVar;

        public IActionResult MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                View();
            }
        }

        [HttpGet]
        [Route("/home/hospital")]
        public IActionResult HospitalIinfo()
        {
            Log.Infomation("HomeController-HospitalIinfo-45", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
            return View("HospitalInfo");
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
