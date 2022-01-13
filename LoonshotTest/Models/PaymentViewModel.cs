using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
//주석추가
namespace LoonshotTest.Models
{
    public class PaymentViewModel
    {
        public string patient_login_id { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public string staff_name { get; set; }
        public int treat_id { get; set; }
        public string prescription { get; set; }
        public int treatment_amount { get; set; }
        public string treat_date { get; set; }
        public string treat_details { get; set; }
        public int disease_id { get; set; }
        public string disease_name { get; set; }

        public PaymentViewModel PaymentView()
        {
            PaymentViewModel paymentViewModel;

            string sql = @"
                SELECT l.patient_login_id, p.patient_name, p.patient_id, t.disease_id, m.staff_name, n.disease_name, t.treat_id, t.prescription, e.treatment_amount, TO_CHAR(t.treat_date, 'YYYY/MM/DD') treat_date, t.treat_details
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
                //Debug.WriteLine("***************디비에서 넘어온 값 : "+paymentViewModel.patient_name);
                //Debug.WriteLine("***************디비에서 넘어온 값 : " + paymentViewModel.prescription);
                //Debug.WriteLine("***************디비에서 넘어온 값 : " + paymentViewModel.staff_name);

            }
            if (paymentViewModel != null)
            {
                return paymentViewModel;
            }
            else
            {
                throw new Exception("결제 정보가 존재하지 않습니다.");
            }




        }
    }
}