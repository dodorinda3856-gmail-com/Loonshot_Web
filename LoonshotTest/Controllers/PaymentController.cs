using LoonshotTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                Debug.WriteLine("페이먼트컨트롤러에서 세션에 담긴 페이션트로그인값:"+HttpContext.Session.GetString("userId"));
                string patient_login_id = HttpContext.Session.GetString("userId");
                // string patient_login_id = User.Identity.Name;
                
                PaymentViewModel paymentView = new PaymentViewModel();

                paymentView.patient_login_id = patient_login_id;

                paymentView.PaymentView();

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
