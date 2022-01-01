using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LoonshotTest.Models
{
    public class TreatMentModel
    {
        public int Treat_id { get; set; }
        public int patient_Id { get; set; }
        public int staff_id { get; set; }
        public int disease_id { get; set; }

        public string disease_name { get; set; }
        public string procedure_name { get; set; }
            
        public string a_s { get; set; }
        public string treat_details{ get; set; }

        public string prescription { get; set; }

        public DateTime treat_date { get; set; }
        public string patient_name { get; set; }

        public string resident_Regist_Num { get; set; }

        public string address { get; set; }

        public String phone_Num { get; set; }

        public DateTime regist_Date { get; set; }

        public string gender { get; set; }

        public DateTime dob { get; set; }

        public string staff_name { get; set; }

        public char agree_Of_Alarm { get; set; }

        public static TreatMentModel GetMyinfo(int patient_Id)
        {
           using(var db = new MySqlDapperHelper())
            {
                string sql = @"
                SELECT p.RESIDENT_REGIST_NUM ,p.ADDRESS,p.PATIENT_NAME ,p.GENDER ,p.PHONE_NUM ,p.DOB ,p.AGREE_OF_ALARM 
                FROM PATIENT p 
                WHERE PATIENT_ID = :patient_Id AND PATIENT_STATUS_VAL = 'T'";
                return db.QuerySingle<TreatMentModel>(sql, new { patient_id = patient_Id });
            }
        }

        public static List<TreatMentModel> TreatmentList(int patient_Id) {
            using (var db = new MySqlDapperHelper()) {
                string sql = @"
                    SELECT  (SELECT ms.STAFF_NAME FROM MEDI_STAFF ms WHERE ms.STAFF_ID = t.STAFF_ID) AS STAFF_NAME , t.TREAT_DETAILS , t.PRESCRIPTION ,t.TREAT_DATE , nod.DISEASE_NAME , mp.PROCEDURE_NAME, mp.A_S 
                    FROM TREATMENT t LEFT JOIN NAME_OF_DISEASE nod ON t.DISEASE_ID = nod.DISEASE_ID 
                    LEFT JOIN MEDI_PROCEDURE mp ON mp.MEDI_PROCEDURE_ID = nod.MEDI_PROCEDURE_ID 
                    WHERE t.PATIENT_ID = : patient_Id
                ";
                return db.Query<TreatMentModel>(sql, new { patient_id = patient_Id });
            }
        }
        public int UserBolt(int patient_id)
        {
            using (var db = new MySqlDapperHelper())
            {
                db.BeginTransaction();

                try
                {
                    string sql = @"
                        UPDATE PATIENT 
                        SET PATIENT_STATUS_VAL = 'F'
                        WHERE PATIENT_ID = : patient_id
                    ";

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

        public int UserAlarm(TreatMentModel param)
        {
            using (var db = new MySqlDapperHelper())
            {
                db.BeginTransaction();
                try
                {
                    string sql = @"
                        UPDATE PATIENT 
                        SET AGREE_OF_ALARM = :agree_Of_Alarm
                        WHERE PATIENT_ID = :patient_id
                    ";

                    int r = 1;
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


        public int MyinfoUpdate(TreatMentModel param, string SqlType) {
            using (var db = new MySqlDapperHelper()) {
                db.BeginTransaction();
                try
                {
                    string choice = null;

                    if (SqlType == "N") { choice = "patient_name = :patient_name,"; }
                    else if (SqlType == "P") { choice = "PHONE_NUM = :phone_num,"; }
                    else if (SqlType == "A") { choice = "ADDRESS = :address,"; }
                    string sql = @"
                        UPDATE PATIENT SET "+ choice + "UPDATE_DATE = CURRENT_TIMESTAMP WHERE patient_id = :patient_id";
                    int r = 1;
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
