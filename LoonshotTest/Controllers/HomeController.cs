using LoonshotTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace LoonshotTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PatientList()
        {
            int patientId = 3;
            return View(PatientModel.GetList(patientId));
        }


        public IActionResult PatientChange([FromForm]PatientModel model)
        {

            model.Update();

            return Redirect("/home/PatientList");
        }

        public IActionResult PatientInsertView()
        {

            return View();
        }

        [HttpGet]
        [Route("/home/hospital")]
        public IActionResult HospitalIinfo()
        {
            return View("HospitalInfo");
        }


        public IActionResult PatientInsert([FromForm] PatientModel model)
        {
            
            model.Insert();

            return Redirect("/home/PatientList");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test(string x, string y)
        {
            ViewData["x"] = x;
            ViewBag.y = y;
            return View();
        }


    }
}
