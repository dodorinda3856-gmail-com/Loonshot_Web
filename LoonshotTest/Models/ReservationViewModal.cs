using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class ReservationViewModal
    {

        public DateTime Reservation_Date { get; set; }
        
        public string Symptom { get; set; }
        //   public MediStaff Doctor { get; set; }
        public string Hour { get; set; }
        public string Staff_Name { get; set; }
        
        public string Patient_Name { get; set; }
        
    }
}
