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
using System.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CoolSms;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace LoonshotTest.Controllers
{
    public class LoginController : Controller
    {

        public static string social_Id = "";       


        public IActionResult Index()
        {
           return View("/login/login");            
        }

        SmsApi api = new SmsApi(new SmsApiOptions
        {
            ApiKey = "NCSV8FXAG2HITRNS",
            ApiSecret = "CNYDVWYSQOTTMEV4V2NQWQ71QKUQS0NC",
            DefaultSenderId = "01020933698" 
        });

        [HttpGet]
        [Route("/login/sendSMS")]
        public string SendSMS(string phone)
        {
            string content = "";
            try
            {
                Debug.WriteLine("+++++++++들어온값 : " + phone);
                Random rand = new Random();

                for (int i = 0; i < 6; i++)
                {
                    int a = rand.Next(1, 10);
                    content += a.ToString();
                }

                Debug.WriteLine("+++++++++인증코드값 : " + content);

                var result = api.SendMessageAsync(phone, "이지피부과 인증코드는 [" + content + "] 입니다.");
            }
            catch (Exception e)
            {
                Log.ERROR(e, "no-login");
            }
            return content; 
        }



        #region 로그인
        [HttpGet]
        [Route("/login/login")]
        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpGet]
        [Route("/login/kakaologin")]
        public IActionResult KakaoLogin(string userId, string kakaoemail, string token)
        {
            social_Id = kakaoemail;
            LoginModel loginModel = new LoginModel();
            int j = 0;

            try
            {
                loginModel.patient_login_id = kakaoemail;
                loginModel.social_id = userId;
                loginModel.status = 'K';
                j = loginModel.SocialCheck();

                HttpContext.Session.SetString("userId", kakaoemail);
                HttpContext.Session.SetString("kakao", "T");
                HttpContext.Session.SetString("token", token);

                return Redirect("/login/AddRegister");
            }
            catch (Exception e)
            {
                Log.FATAL(e, "no-login");
                return Redirect("/");
            }
        }


        public IActionResult AddRegister()
        {
            Debug.WriteLine("inside add register");
            return View();
        }


        [HttpPost]
        [Route("/login/socialRegister")]
        public IActionResult socialRegister([FromForm] LoginModel input)
        {
            try {
                input.patient_login_id = social_Id;
                input.SocialRegister();
            }
            catch (Exception e) {
                Log.ERROR(e, "no-login");
            }

            return Redirect("/");
        }

        [HttpPost]
        [Route("/login/login")]
        public async Task <IActionResult> LoginProc([FromForm]LoginModel input)
        {
            try
            {

                input.ConvertPassword();
                var Login = input.GetLoginUser();

                HttpContext.Session.SetString("userId", Login.patient_login_id);
                Log.Infomation("LoginController-LoginProc", Login.patient_login_id);

                return Redirect("/");
            }
            catch(Exception e)
            {
                Log.ERROR(e, "fail-login");
                return Redirect($"/login/login?msg={HttpUtility.UrlEncode(e.Message)}");
            }
    
        }
        #endregion 

        #region 회원가입
        public IActionResult Register(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        [Route("/login/idCheck")]
        public ActionResult idCheck(string id)
        
        {
            LoginModel loginModel = new LoginModel();
            loginModel.patient_login_id = id;
          if(id == null)
            {
                return Content("");
            }
          if(id.Length < 5 || id.Length > 15)
            {
                return Content("아이디는 5자리 이상 15자리 이하만 가능합니다.");
            }
          else
            {
                try
                {
                    loginModel.UserCheck(id);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                
                return Content("사용가능한 아이디입니다.");
            }
        }

        [HttpPost]
        [Route("login/pwCheck")]
        public ActionResult pwCheck(string pw)
        {
            Regex rxPassword = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{9,}$",
                        RegexOptions.IgnorePatternWhitespace);
            if(pw == null)
            {
                return Content("");
            }
            if (!rxPassword.IsMatch(pw))
            {
                Debug.WriteLine("비밀번호가 안맞아");
                return Content("비밀번호는 8자리 이상의 영문, 숫자, 특수문자가 포함되어야 합니다.");
            }
            else
            {
                return Content("사용 가능한 패스워드입니다.");
            }
        }


        [HttpPost]
        [Route("login/pwCheck2")]
        public ActionResult pwCheck2(string status)
        {
            if(status == null)
            {
                return Content("");
            }
            if(status == "F")
            {
                return Content("패스워드가 동일하지 않습니다.");
            }
            else
            {
                return Content("패스워드가 일치합니다.");
            }
            
        }


        [HttpPost]
        [Route("login/rsCheck")]
        public ActionResult rsCheck(string rs)
        {
            Regex rxRs = new Regex(@"[0-9]",
                        RegexOptions.IgnorePatternWhitespace);
            if(rs == null)
            {
                return Content("");
            }
            
            if(rxRs.IsMatch(rs) && rs.Length == 13)
            {
                return Content("유효한 주민등록번호 입니다.");
            }
            else
            {
                return Content("주민등록번호가 유효하지 않습니다.");
            }
        }




        [HttpPost]
        [Route("/login/register")]
        public IActionResult RegisterProc([FromForm] LoginModel input)
        {
           try
            {
                input.UserCheck(input.patient_login_id);
                input = null;
                string patient_login_pw2 = Request.Form["patient_login_pw2"];

                if(input.patient_login_id.Length < 5 || input.patient_login_id.Length > 14)
                {
                    throw new Exception("ID는 5자리 이상 14자리 이하까지 가능합니다.");
                }

                if(input.patient_login_pw.Length < 6 || input.patient_login_pw.Length > 15)
                {
                    throw new Exception("패스워드는 6자리 이상, 15자리 이하만 가능합니다.");
                }
                
                if (input.patient_login_pw != patient_login_pw2)
                {
                    throw new Exception("패스워드가 불일치 합니다.");
                }
                */

                input.status = 'L';
                input.ConvertPassword();
                input.checkPhone();
                input.Register();
                Log.Infomation("LoginContorller-RegisterProc", "no-login");
                return Redirect("/login/login");
            }
            catch (Exception e)
            {
                Log.ERROR(e, "no-login");
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(e.Message)}");
                //return Redirect("/login/register");
            }
        }
        #endregion 

        public async Task<IActionResult> LogOut()
        {
            Debug.WriteLine("******************토큰값:"+ HttpContext.Session.GetString("token"));
            
            Debug.WriteLine("들어왔니?");
            LoonshotTest.Log.Infomation("LoginController-LoginProc", HttpContext.Session.GetString("userId"));
            HttpContext.Session.Clear();
            //await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
