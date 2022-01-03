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
using System.Web;

namespace LoonshotTest.Controllers
{
    [CheckUser]
    public class MypageController : Controller
    {

        private readonly ILogger<MypageController> _logger;

        public MypageController(ILogger<MypageController> logger)
        {
            _logger = logger;
            
        }

        [Route("/mypage/info")]
        public IActionResult Mypage() {

            try
            {
                if (User.Identity.Name == null)
                {
                    throw new Exception("로그인이 필요한 서비스입니다.");
                }


                LoginModel loginmodel = new LoginModel();
                loginmodel.patient_login_id = User.Identity.Name;
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
                TreatMentModel myinfo = TreatMentModel.GetMyinfo(loginmodel.patient_id);
                List<TreatMentModel> treatList = TreatMentModel.TreatmentList(loginmodel.patient_id);

                return View(Tuple.Create(myinfo, treatList));
            }
            catch (Exception ex) {
                return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }

        [Route("/mypage/UserSecession")]
        public IActionResult MypageUserRemove() {
            LoginModel loginmodel = new LoginModel();
            
            loginmodel.UserBolt(User.Identity.Name);
            HttpContext.SignOutAsync();

            return Redirect("/");
        }

        [HttpPost]
        public JsonResult ChangeAlarm(string AGREE_OF_ALARM) {
            LoginModel loginmodel = new LoginModel(); 
            loginmodel.patient_login_id = User.Identity.Name;
            loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
            TreatMentModel alarmStat = new TreatMentModel();
            alarmStat.patient_Id = loginmodel.patient_id;
            alarmStat.agree_Of_Alarm = (AGREE_OF_ALARM == "true" ? 'T' : 'F');
            string message = "succces";
            if (alarmStat.UserAlarm(alarmStat) != 1) {
                message = "Error";
            }

            return new JsonResult(new { Message = message, System.Web.Mvc.JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateUser(string SqlType, string ajaxData)
        {
            LoginModel loginmodel = new LoginModel();
            loginmodel.patient_login_id = User.Identity.Name;
            loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);

            TreatMentModel userinfo = new TreatMentModel();
            string sql_choice = SqlType;

            if (sql_choice == "N")
            { userinfo.patient_name = ajaxData; }
            else if (sql_choice == "P") { userinfo.phone_Num = ajaxData; }
            else if (sql_choice == "A") { userinfo.address = ajaxData; }
            userinfo.patient_Id = loginmodel.patient_id;

            string message = "succces";

            if (userinfo.MyinfoUpdate(userinfo, SqlType) == 1)
            {
                message = "error";
            }

            return new JsonResult(new { Message = message, System.Web.Mvc.JsonRequestBehavior.AllowGet });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("/mypage/diagnosis")]
        public IActionResult diagnosis()
        {
            return View();
        }
    }
}
