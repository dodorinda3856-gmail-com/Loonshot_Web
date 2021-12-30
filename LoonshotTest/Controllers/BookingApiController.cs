using LoonshotTest.Models;
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
    [Route("api/[controller]")]
    [ApiController]
    public class BookingApiController : ControllerBase
    {
        [HttpGet("Appointment")]
        public IEnumerable<Reservation> Appointment(
            [FromQuery] string doctorId,
           [FromQuery] string date)
        {

            return ReservationService.GetBookedPatients(doctorId, date);

        }

        [HttpGet("Schedule")]
        public IEnumerable<Time> Schedule(
            [FromQuery] int day)
        {
            Debug.WriteLine(day);
            return ReservationService.GetTimeTable(day);
        }

    }
}


