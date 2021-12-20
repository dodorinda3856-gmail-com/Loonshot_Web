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
            string test =  HttpContext.User.Identity.Name;
            
            Models.Login.LoginModel loginUserInfo = new Models.Login.LoginModel();
            loginUserInfo.patient_login_id = User.Identity.Name;
            loginUserInfo = loginUserInfo.GetUserId();

            TreatMentModel myinfo = TreatMentModel.GetMyinfo(loginUserInfo.patient_id);
            List<TreatMentModel> treatList = TreatMentModel.TreatmentList(loginUserInfo.patient_id);

            return View(Tuple.Create(myinfo, treatList));
        }

        [Route("/mypage/UserSecession")]
        public IActionResult MypageUserRemove() {
            Models.Login.LoginModel loginUserInfo = new Models.Login.LoginModel();
            loginUserInfo.patient_login_id = User.Identity.Name;
            loginUserInfo = loginUserInfo.GetUserId();
            loginUserInfo.UserSecession(loginUserInfo.patient_id);

            HttpContext.SignOutAsync();

            return Redirect("/");
        }

        [HttpPost]
        public JsonResult ChangeAlarm(string AGREE_OF_ALARM) {
            TreatMentModel alarmStat = new TreatMentModel();
            alarmStat.patient_Id = 2;
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
            Models.Login.LoginModel loginUserInfo = new Models.Login.LoginModel();
            loginUserInfo.patient_login_id = User.Identity.Name;
            loginUserInfo = loginUserInfo.GetUserId();

            TreatMentModel userinfo = new TreatMentModel();
            string sql_choice = SqlType;

            if (sql_choice == "N")
            { userinfo.patient_name = ajaxData; }
            else if (sql_choice == "P") { userinfo.phone_Num = ajaxData; }
            else if (sql_choice == "A") { userinfo.address = ajaxData; }
            userinfo.patient_Id = loginUserInfo.patient_id;

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
    }
}
