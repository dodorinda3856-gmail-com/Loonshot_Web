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
            for (int i = 0; i < reservationDays; i++)
            {
                dateList.Add(days.AddDays(i + 1));
            }
            ViewData["Days"] = dateList;
            ViewData["Doctor"] = doctor;
            ViewBag.userId = 2;
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
        public ActionResult ChooseDate(
            int medical_staff_id,
            string reservation_date,
            string symptom,
            int time_Id)
        {
            
                var reservationInfo=ReservationService.AddReservation(2, medical_staff_id, reservation_date, symptom, time_Id);

            return RedirectToAction(nameof(Info), new { docName = reservationInfo.Staff_Name, date = reservationInfo.Reservation_Date,
                hour = reservationInfo.Hour, symptom = reservationInfo.Symptom, patientName=reservationInfo.Patient_Name });
    
        }

    }
}
