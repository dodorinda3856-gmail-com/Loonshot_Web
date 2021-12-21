using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class PaymentModel
    {
        public int payment_id { get; set; }

        public int patient_id { get; set; }

        public int treat_id { get; set; }

        public int disease_id { get; set; }

        public string payment_date { get; set; }

        public int origin_amount { get; set; }

        public int discount_amount { get; set; }

        public int fin_payment_amount { get; set; }

        public string creation_date { get; set; }

        public string revised_date { get; set; }

        public string delete_date { get; set; }

        public char delete_or_not { get; set; }


        public int Insert()
        {
            string sql = @"
                    INSERT INTO PAYMENT (
                    payment_id, patient_id, treat_id, disease_id, payment_date, origin_amount,
                    discount_amount, fin_payment_amount, creation_date, revised_date, delete_date, delete_or_not
                    ) VALUES (PAYMENT_SEQ.NEXTVAL, :patient_id, :treat_id, :disease_id, CURRENT_TIMESTAMP, :origin_amount,
                    :discount_amount, :fin_payment_amount, CURRENT_TIMESTAMP, :revised_date, :delete_date, :delete_or_not) ";


            string sql2 = @"
                        UPDATE treatment
                        SET
                        treat_status__val = 'F'
                        where 
                        treat_id = :treat_id
                        ";

            using (var db = new MySqlDapperHelper())
            {
                db.Execute(sql2, this);
                return db.Execute(sql, this);
                
            }

        }



    }
}
