using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class PatientModel
    {
        public int patient_Id { get; set; }

        public string patient_login_id { get; set; }
        public string patient_name { get; set; }

        public string resident_Regist_Num { get; set; }

        public string address { get; set; }

        public String phone_Num { get; set; }

        public DateTime regist_Date { get; set; }

        public char gender { get; set; }

        public DateTime dob { get; set; }

        public char patient_Status_Val { get; set; }

        public char agree_Of_Alarm { get; set; }

        public static List<PatientModel> GetList(int patient_Id)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = @"
                SELECT * FROM PATIENT WHERE patient_id = :patient_id";
                return db.Query<PatientModel>(sql, new { patient_id = patient_Id });
            }
        }

        public int Insert()
        {
            string sql = @"
                        INSERT INTO patient(patient_id, resident_regist_num, address, 
                        patient_name, phone_num, regist_date, gender, dob, patient_status_val, agree_of_alarm)
                        VALUES(:patient_id, :resident_regist_num, :address, :patient_name, :phone_num, CURRENT_TIMESTAMP,
                        :gender, TO_DATE('1971-03-31', 'YYYY-MM-DD'), :patient_status_val, :agree_of_alarm) ";


            using (var db = new MySqlDapperHelper())
            {
                return db.Execute(sql, this);
            }

        }
        public int Update()
        {

            using (var db = new MySqlDapperHelper())
            {
                db.BeginTransaction();

                try
                {
                    string sql = @"
                        UPDATE PATIENT SET patient_name = :patient_name
                        WHERE patient_id = :patient_id";
                    int r = 0;
                    r += db.Execute(sql, this);
                    r += db.Execute(sql, this);
                    r += db.Execute(sql, this);

                    db.Commit();

                    return r;
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    throw ex;
                }


            }
        }
    }
}