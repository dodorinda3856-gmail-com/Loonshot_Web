using LoonshotTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
            try
            {
                string patient_login_id = User.Identity.Name;

                PaymentViewModel paymentView = new PaymentViewModel();

                paymentView.patient_login_id = patient_login_id;

                ViewData["paymentView"] = paymentView.PaymentView();
                return View();
            }catch(Exception ex)
            {
                return Redirect("/payment/NoPayment");
            }
            
        }


        [Route("/payment/NoPayment")]
        public IActionResult NoPayment()
        {
            return View();
        }


        [HttpPost]
        [Route("/payment/PaymentInsert")]
        public IActionResult PaymentInsert(PaymentModel paymentModel)
        {
          
            paymentModel.Insert();
            return Content("결제가 성공하였씁니다.");
        }
    }
}
