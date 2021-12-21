using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class PaymentViewModel
    {
        public string patient_login_id { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public string staff_name { get; set; }
        public string prescription { get; set; }
        public int treatment_amount { get; set; }
        public string treat_date { get; set; }


        public PaymentViewModel PaymentView()
        {
            PaymentViewModel paymentViewModel;

            string sql = @"
                SELECT p.patient_name, m.staff_name, t.prescription, e.treatment_amount, TO_CHAR(t.treat_date, 'YYYY/MM/DD')
                FROM patient_login l, patient p, treatment t, medi_staff m, name_of_disease n, medi_procedure e
                WHERE l.patient_login_id = :patient_login_id AND
                l.patient_id = p.patient_id AND
                p.patient_id = t.patient_id AND
                t.staff_id = m.staff_id AND
                t.disease_id = n.disease_id AND
                n.medi_procedure_id = e.medi_procedure_id AND
                treat_status__val = 'T'
                ";

            using (var db = new MySqlDapperHelper())
            {

                paymentViewModel = db.QuerySingle<PaymentViewModel>(sql, this);
                Debug.WriteLine("***************디비에서 넘어온 값 : "+paymentViewModel);
            }


            return paymentViewModel;
        }


    }

    
}
