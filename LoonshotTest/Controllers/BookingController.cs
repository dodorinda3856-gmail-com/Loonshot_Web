using LoonshotTest.Models.Login;
using LoonshotTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Controllers
{
    public class BookingController : Controller
    {
        // GET: BookingController
        public ActionResult Index()
        {
            var data = MediStaffService.GetList();
            data=data.FindAll(v => v.Staff_Name != "-");
            return View(data);
        }

        public ActionResult ChooseDate(int id)
        {
            try
            {
                var reservationRecord = ReservationService.GetList(id);
                //   ViewData["TimeTable"] = ReservationService.GetTimeTableList();
                var doctor = MediStaffService.GetDoctorInfo(id);
                if (doctor == null)
                {
                    throw new Exception("잘못된 choose date 의사 접근. 아이디값=" + id);
                }
                List<DateTime> dateList = new List<DateTime>();
                int reservationDays = 7;
                var days = DateTime.Now.Date;
                LoginModel loginmodel = new LoginModel();
                loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);

                if(loginmodel == null)
                {
                    return RedirectToAction("login", "login", new { msg = "로그인이 필요한 서비스입니다." });
                }

                for (int i = 0; i < reservationDays; i++)
                {
                    dateList.Add(days.AddDays(i + 1));
                }
                ViewData["Days"] = dateList;
                ViewData["Doctor"] = doctor;
                ViewBag.userId = loginmodel.patient_id;

                // 사용자의 사이트 이동경로 파악 로그
                // Infomation("해당 경로", 로그인한 유저의 아이디) 값 매개변수로 설정
                // 아래와 같이 올바르게 실행되는 결과 try 문의 가장 하단에 반환(return) 전에 해당 메서드를 추가
                Log.Infomation("BookingController-ChooseDate"+id, HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");



                //catch (Exception e) { }
                //실패의 경우 에는 e에 실패에 대한 경로, 원인, 해당 레벨 등이 자동으로 DB의 저장되므로 아래와 같이 catch 에 해당 메소드만 추가 해주면 됩니다.
                //Log.ERROR(e, HttpContext.Session.GetString("userId"));


                return View(ReservationService.GetList(id));
            }
            catch(Exception err)
            {
                Log.ERROR(err, HttpContext.Session.GetString("userId"));
                return Redirect("~/Views/Error/Error.cshtml");
            }
        }

        public ActionResult Info(DateTime date, string docName, string hour, string symptom, string patientName)
        {
            ViewBag.docName = docName;
            ViewBag.date = date;
            ViewBag.hour = hour;
            ViewBag.symptom = symptom;
            ViewBag.patientName = patientName;
            return View();
        }

        public ActionResult Delete(int id)
        {
           
            ViewBag.Id = id;
            LoginModel loginmodel = new LoginModel();
            loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
            loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
            

            ReservationService.DeleteMyReservation(loginmodel.patient_id, id);
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        public ActionResult MyBooking()
        {
            try
            {
                DateTime today = DateTime.Now.Date;
                LoginModel loginmodel = new LoginModel();
                loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
                loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);

                if (loginmodel == null)
                {
                    return RedirectToAction("login", "login", new { msg = "로그인이 필요한 서비스입니다."});
                }

                var reservationRecord = ReservationService.GetMyReservation(loginmodel.patient_id);
                
                Log.Infomation("BookingController-Mybooking", HttpContext.Session.GetString("userId") != null ? HttpContext.Session.GetString("userId") : "no-login");

                return View(reservationRecord);
            }
            catch (Exception err)
            {
                Log.ERROR(err, HttpContext.Session.GetString("userId"));
                return Redirect("~/Views/Error/Error.cshtml");
            }
           
        }


        // POST: BookingController/Create
        [HttpPost]
        public ActionResult ChooseDate(
            int medical_staff_id,
            string reservation_date,
            string symptom,
            string treat_type,
            int time_Id)
        {
            LoginModel loginmodel = new LoginModel();
            loginmodel.patient_login_id = HttpContext.Session.GetString("userId");
            loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
            Debug.WriteLine("reservationd date" + reservation_date);
            var reservationInfo=ReservationService.AddReservation(loginmodel.patient_id, medical_staff_id, reservation_date, treat_type, symptom, time_Id);
            
            return RedirectToAction(nameof(Info), new { docName = reservationInfo.Staff_Name, date = reservationInfo.Reservation_Date,
                hour = reservationInfo.Hour, symptom = reservationInfo.Symptom, patientName=reservationInfo.Patient_Name });
        }

    }
}
