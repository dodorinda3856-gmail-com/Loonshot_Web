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
            ApiKey = "NCS8MFAZLUUNTIAC",
            ApiSecret = "5GRHMBBAWFN72IOQ2QHBIEACJVNEPY1Z",
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
        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpGet]
        [Route("/login/kakaologin")]
        public async Task <IActionResult> KakaoLogin(string userId, string kakaoemail)
        {
            
            Debug.WriteLine("+++++++++들어온값 : " + userId);
            Debug.WriteLine("+++++++++들어온값 : " + kakaoemail);
            
            social_Id = userId;
            LoginModel loginModel = new LoginModel();
            int j = 0;

            loginModel.patient_login_id = userId;
            loginModel.status = 'K';
             j = loginModel.SocialCheck();


            //로그인작업
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            identity.AddClaim(new Claim(ClaimTypes.Email, kakaoemail));
            identity.AddClaim(new Claim(ClaimTypes.Name, kakaoemail));
            identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMDDHHmmss")));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                AllowRefresh = true
            });
            if(j == 1)
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
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.UserData);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Login.patient_login_id));
                identity.AddClaim(new Claim(ClaimTypes.Name, Login.patient_login_id));
                identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMDDHHmmss")));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    AllowRefresh = true
                });
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
            }
        }
        #endregion 

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
