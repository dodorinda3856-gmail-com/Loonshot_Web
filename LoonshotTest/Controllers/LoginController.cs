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

namespace LoonshotTest.Controllers
{
    public class LoginController : Controller
    {
        
        public LoginController()
        {

        }


        public IActionResult Index()
        {
            return View("/login/login");
        }

        
        #region 로그인
        [HttpGet]
        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
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
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
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
                
                input.ConvertPassword();

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
