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


        public LoginController()
        {

        }


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
            Debug.WriteLine("+++++++++들어온값 : " + phone);
            Random rand = new Random();
            string content = "";

            for(int i =0; i < 6; i++)
            {
                int a = rand.Next(1, 10);
                content += a.ToString();
            }
            
            Debug.WriteLine("+++++++++인증코드값 : " + content);

            var result = api.SendMessageAsync(phone, "이지피부과 인증코드는 ["+content+"] 입니다.");
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
        public async Task <IActionResult> KakaoLogin(string userId, string kakaoemail, string token)
        {
            
            Debug.WriteLine("+++++++++들어온값 : " + userId);
            Debug.WriteLine("+++++++++들어온값 : " + kakaoemail);
            Debug.WriteLine("+++++++++들어온토큰값 : " + token);
            social_Id = kakaoemail;
            LoginModel loginModel = new LoginModel();
            int j = 0;

            loginModel.patient_login_id = kakaoemail;
            loginModel.social_id = userId;
            loginModel.status = 'K';
             j = loginModel.SocialCheck();


            //로그인작업
            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            //identity.AddClaim(new Claim(ClaimTypes.Email, kakaoemail));
            //identity.AddClaim(new Claim(ClaimTypes.Name, kakaoemail));
        
            //identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMDDHHmmss")));

            //Debug.WriteLine("+++++++++들어온값클래임+++ : " + identity.FindFirst(ClaimTypes.NameIdentifier));

            //identity.FindFirst(ClaimTypes.Email);

            //var principal = new ClaimsPrincipal(identity);
            HttpContext.Session.SetString("userId", kakaoemail);
            HttpContext.Session.SetString("kakao", "T");
            HttpContext.Session.SetString("token", token);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            //{
            //    IsPersistent = false,
            //    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
            //    AllowRefresh = true
            //});
            if (j == 1)
            {
                //return Response.Redirect(string.Format("/login/AddRegister"));
                //byte[] utf8Bytes = Encoding.UTF8.GetBytes("/login/AddRegister");
                Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                
                return Redirect("/login/AddRegister");
            }
            else
            {       
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
            input.patient_login_id = social_Id;
            input.SocialRegister();

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

                //로그인작업
                //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.UserData);
                //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Login.patient_login_id));
                //identity.AddClaim(new Claim(ClaimTypes.Name, Login.patient_login_id));
                //identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMDDHHmmss")));

                //var principal = new ClaimsPrincipal(identity);

                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                //{
                //    IsPersistent = false,
                //    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                //    AllowRefresh = true
                //});
                HttpContext.Session.SetString("userId", Login.patient_login_id);
                //Debug.WriteLine("쎼쎤값********************"+HttpContext.Session.GetString("userId"));
                //Debug.WriteLine("********************프린시팔값:"+principal);
                //Debug.WriteLine("********************프린씨팔의 네임 값" + principal.Identity.Name);
            
                return Redirect("/");
            }
            catch(Exception ex)
            {
                return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
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
                input.status = 'L';
                input.ConvertPassword();
                input.checkPhone();
                input.Register();
            
                return Redirect("/login/login");
            }
            catch (Exception ex)
            {
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");
                //return Redirect("/login/register");
            }
        }
        #endregion 

        public async Task<IActionResult> LogOut()
        {
            Debug.WriteLine("******************토큰값:"+ HttpContext.Session.GetString("token"));
            
            Debug.WriteLine("들어왔니?");
            HttpContext.Session.Clear();
            //await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
