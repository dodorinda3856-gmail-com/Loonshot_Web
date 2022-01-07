using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class MyReservationRecord
    {
        public int Reservation_Id { get; set; }
        public int Patient_Id { get; set; }
        public DateTime Reservation_Date { get; set; }
        public int Time_Id { get; set; }
        //   public Time Time { get; set; }
        public string Hour { get; set; }
        public int Medical_Staff_Id { get; set; }
        public string Staff_Name { get; set; }
        public string Medi_Subject { get; set; }
        public string Symptom { get; set; }
        //   public MediStaff Doctor { get; set; }

    }
}
