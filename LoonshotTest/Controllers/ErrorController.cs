using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {

        }

        [Route("/Error/{ErrorCode}")]
        public IActionResult Error(int ErrorCode)
        {
            Debug.WriteLine("에러콘트롤러 진입******************");
            Debug.WriteLine("에러코드명 : " + ErrorCode);
            switch (ErrorCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "유효하지 않은 페이지로 접근하셨습니다.";
                    ViewBag.ErrorNumber = "404";
                    break;
                case 405:
                    ViewBag.ErrorMessage = "유용하지 않은 메서드가 호출되었습니다.";
                    ViewBag.ErrorNumber = "405";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "접근이 불가한 페이지입니다.";
                    ViewBag.ErrorNumber = "401";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "내부 서버 오류입니다.";
                    ViewBag.ErrorNumber = "500";
                    break;
                case 403:
                    ViewBag.ErrorMessage = "제한된 페이지입니다.";
                    ViewBag.ErrorNumber = "403";
                    break;
                case 503:
                    ViewBag.ErrorMessage = "유용하지 않은 서비스입니다.";
                    ViewBag.ErrorNumber = "503";
                    break;
                case 504:
                    ViewBag.ErrorMessage = "게이트웨이가 만료되었습니다.";
                    ViewBag.ErrorNumber = "504";
                    break;
                case 001:
                    ViewBag.ErrorMessage = "만료된 페이지입니다.";
                    ViewBag.ErrorNumber = "Oh no!";
                    break;
            }
            return View();
        }
    }
}
