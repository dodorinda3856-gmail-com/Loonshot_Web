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
using System.Web.Mvc;

namespace LoonshotTest.Controllers
{
    public class MypageController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<MypageController> _logger;

        public MypageController(ILogger<MypageController> logger)
        {
            _logger = logger;
        }

        public IActionResult Mypage() {
            TreatMentModel myinfo = TreatMentModel.GetMyinfo(2);
            List<TreatMentModel> treatList = TreatMentModel.TreatmentList(2);

            return View(Tuple.Create(myinfo, treatList));
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

            return new JsonResult(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateUser(string SqlType, string ajaxData)
        {
            TreatMentModel userinfo = new TreatMentModel();
            string sql_choice = SqlType;

            if (sql_choice == "N")
            { userinfo.patient_name = ajaxData; }
            else if (sql_choice == "P") { userinfo.phone_Num = ajaxData; }
            else if (sql_choice == "A") { userinfo.address = ajaxData; }
            userinfo.patient_Id = 2;

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
