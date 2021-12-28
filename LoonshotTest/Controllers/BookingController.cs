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
            return View(data);
        }

        public ActionResult ChooseDate(int id)
        {
            var reservationRecord = ReservationService.GetList(id);
            //   ViewData["TimeTable"] = ReservationService.GetTimeTableList();
            var doctor = MediStaffService.GetDoctorInfo(id);
            List<DateTime> dateList = new List<DateTime>();
            int reservationDays = 7;
            var days = DateTime.Now.Date;
            LoginModel loginmodel = new LoginModel();
            loginmodel.patient_login_id = User.Identity.Name;
            loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
            for (int i = 0; i < reservationDays; i++)
            {
                dateList.Add(days.AddDays(i + 1));
            }
            ViewData["Days"] = dateList;
            ViewData["Doctor"] = doctor;
            ViewBag.userId = loginmodel.patient_id;
            return View(ReservationService.GetList(id));
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


        // POST: BookingController/Create
        [HttpPost]
        public void ChooseDate(
            int medical_staff_id,
            string reservation_date,
            string symptom,
            int time_Id)
        {

            LoginModel loginmodel = new LoginModel();
            loginmodel.patient_login_id = User.Identity.Name;
            loginmodel = loginmodel.GetUserInfo(loginmodel.patient_login_id);
            Debug.WriteLine("reservationd date" + reservation_date);
            var reservationInfo=ReservationService.AddReservation(loginmodel.patient_id, medical_staff_id, reservation_date, symptom, time_Id);
            /*
            return RedirectToAction(nameof(Info), new { docName = reservationInfo.Staff_Name, date = reservationInfo.Reservation_Date,
                hour = reservationInfo.Hour, symptom = reservationInfo.Symptom, patientName=reservationInfo.Patient_Name });
    */
         
        }

    }
}
