using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class Reservation
    {
        public int Reservation_Id { get; set; }
        public int Patient_Id { get; set; }
        public DateTime Reservation_Date { get; set; }
        public int Time_Id { get; set; }
     //   public Time Time { get; set; }
        public int Medical_Staff_Id { get; set; }

        public string Symptom { get; set; }
     //   public MediStaff Doctor { get; set; }

    }
}
