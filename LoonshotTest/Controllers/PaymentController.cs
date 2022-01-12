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

                Log.Infomation("PaymentController-Payment()-23", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
                return View();

            }
            catch(Exception ex)
            {
                Log.ERROR(ex, HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
                return Redirect("/payment/NoPayment");
            }
            
        }


        [Route("/payment/NoPayment")]
        public IActionResult NoPayment()
        {
            Log.Infomation("PaymentController-Payment()-23", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
            return View();
        }


        [HttpPost]
        [Route("/payment/PaymentInsert")]
        public IActionResult PaymentInsert(PaymentModel paymentModel)
        {
            try { 
                Log.Infomation("PaymentController-Payment()-23", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
                paymentModel.Insert();
                return Content("결제가 성공하였씁니다.");
            }
            catch (Exception e) {
                Log.ERROR(e, HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
                return Content("결제가 실패했니다.");
            }
        }
    }
}
