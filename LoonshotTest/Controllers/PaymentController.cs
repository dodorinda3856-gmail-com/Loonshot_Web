using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Controllers
{
    
    public class PaymentController : Controller
    {

        public PaymentController()
        {

        }
        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

    }
}
