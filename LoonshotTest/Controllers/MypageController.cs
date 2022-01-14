using LoonshotTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Dynamic;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using Microsoft.AspNetCore.Authentication;
using LoonshotTest.Controllers;
using LoonshotTest.Filters;
using LoonshotTest.Models.Login;
using System.Web.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Net;

namespace LoonshotTest.Controllers
{
    public class MypageController : Controller
    {
        private readonly ILogger<MypageController> _logger;

        public MypageController(ILogger<MypageController> logger)
        {
            _logger = logger;
        }
        private void LoginCheck( ) {
            if (HttpContext.Session.GetString("userId").Equals("") && HttpContext.Session.GetString("userId") == null)
            {
                throw new Exception("로그인이 필요한 서비스입니다.");
            }
        }

        [Route("/mypage/info")]
        public IActionResult Mypage() {
            try
            {
                LoginCheck();
                LoginModel loginmodel = new LoginModel();
                loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
                TreatMentModel myinfo = TreatMentModel.GetMyinfo(loginmodel.patient_id);
                Log.Infomation("MypageController-Mypage()-50", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");
                return View(Tuple.Create(myinfo));
            }
            catch (Exception e) {
                Log.ERROR(e, HttpContext.Session.GetString("userId"));
                var encodedLocationName = WebUtility.UrlEncode("로그인이 필요한 서비스 입니다.");
                return Redirect($"/login/login?msg="+encodedLocationName);
            }
        }

        [Route("/mypage/UserSecession")]
        public IActionResult MypageUserRemove() {
            try
            {
                LoginModel loginmodel = new LoginModel();
                Log.Infomation("MypageController-MypageUserRemove()-64", HttpContext.Session.GetString("userId"));

                loginmodel.UserBolt(HttpContext.Session.GetString("userId"));
                HttpContext.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception e) {
                Log.ERROR(e, HttpContext.Session.GetString("userId"));
                return Redirect("/ERROR/500");
            }
        }

        [HttpPost]
        public JsonResult UserPrescription(int treat_id)
        {
            TreatMentModel treatModel = new TreatMentModel();
            treatModel.Treat_id = treat_id;
            string message = "succces";
            try
            {
                treatModel = treatModel.Prescription(treat_id);

                Log.Infomation("MypageController-MypageUserRemove()-64", HttpContext.Session.GetString("userId"));
                return Json(new
                {
                    name = treatModel.patient_name,
                    resident_num = treatModel.resident_Regist_Num,
                    produce = treatModel.names,
                    print_id = treatModel.print_id,
                    disease_code = treatModel.disease_code,
                    procedure = treatModel.procedure,
                    message = message
                });
            }
            catch (Exception e) {
                message = "Error";
                Log.ERROR(e, HttpContext.Session.GetString("userId"));

                return Json(new
                {
                    name = treatModel.patient_name,
                    resident_num = treatModel.resident_Regist_Num,
                    produce = treatModel.names,
                    print_id = treatModel.print_id,
                    disease_code = treatModel.disease_code,
                    message = message
                });
            } 
        }
        [HttpPost]
        public JsonResult ChangeAlarm(string AGREE_OF_ALARM) {
            LoginModel loginmodel = new LoginModel();
            string message = "succes";
            try
            {
                loginmodel.patient_login_id = HttpContext.Session.GetString("userId"); ;
                if (loginmodel.UserAlarm(loginmodel, (AGREE_OF_ALARM == "true" ? "T" : "F")) != 1)
                {
                    message = "succes";
                }
                Log.Infomation("MypageController-ChangeAlarm()-113", HttpContext.Session.GetString("userId"));

            }
            catch (Exception e){
                message = "500";
                Log.ERROR(e, HttpContext.Session.GetString("userId"));
            }
            return new JsonResult(new { Message = message, System.Web.Mvc.JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateUser(string SqlType, string ajaxData)
        {
            LoginModel loginmodel = new LoginModel();
            loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
            TreatMentModel userinfo = new TreatMentModel();
            string sql_choice = SqlType;
            string message = "succces";

            try
            { 
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
                if (sql_choice == "N")
                { userinfo.patient_name = ajaxData; }
                else if (sql_choice == "P") { userinfo.phone_Num = ajaxData; }
                else if (sql_choice == "A") { userinfo.address = ajaxData; }
                userinfo.patient_Id = loginmodel.patient_id;
                if (userinfo.MyinfoUpdate(userinfo, SqlType) == 1)
                {
                    message = "500";
                }
                Log.Infomation("MypageController-UpdateUser()-133", HttpContext.Session.GetString("userId"));

            }
            catch (Exception e) {
                Log.ERROR(e, HttpContext.Session.GetString("userId"));
                message = "500";
            }
            return new JsonResult(new { Message = message, System.Web.Mvc.JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult GetHistory(int rcnt)
        {
            try
            {
                //LoginCheck();
                LoginModel loginmodel = new LoginModel();
                loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
                List<TreatMentModel> treatList = TreatMentModel.TreatmentList(loginmodel.patient_id, rcnt);
                Log.Infomation("MypageController-GetHistory()-163", HttpContext.Session.GetString("userId"));
                return Json(new
                {
                    list = treatList
                });
            }
            catch (Exception e)
            {
                Log.ERROR(e, HttpContext.Session.GetString("userId"));
                return null;
            }
        }
        [HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult GetAS(int rcnt)
        {
            try
            {
                LoginModel loginmodel = new LoginModel();
                loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
                List<ASModel> asList = ASModel.UserAS(loginmodel.patient_id, rcnt);
                Log.Infomation("MypageController-ActionResult()-182", HttpContext.Session.GetString("userId"));
                return Json(new
                {
                    list = asList
                });
            }
            catch (Exception e)
            {
                Log.ERROR(e, HttpContext.Session.GetString("userId"));
                return null;
            }
        }
    }
}
