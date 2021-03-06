using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LoonshotTest.Models
{
    public class ASModel
    {
        public char as_type { get; set; }
        public string a_s {get; set;}
        public string procedure { get; set; }

        public static List<ASModel> UserAS(int patient_id, int cnt)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = @"
                        SELECT rn , tt.*
                        FROM (
                        SELECT ROWNUM rn , t.*
                        FROM (
                    	SELECT 'D' AS AS_TYPE , nod.A_S , nod.DISEASE_NAME AS procedure , t.treat_date
                        FROM (SELECT t.TREAT_ID, td.DISEASE_ID , t.treat_date FROM TREATMENT t LEFT JOIN TREAT_DISEASE td ON t.TREAT_ID = td.TREATMENT_ID WHERE t.PATIENT_ID = :patient_id AND td.TREATMENT_ID IS NOT NULL) t 
                        LEFT JOIN NAME_OF_DISEASE nod ON t.DISEASE_ID = nod.DISEASE_ID  
                        UNION ALL
                        SELECT 'M' AS AS_TYPE , mp.A_S, mp.PROCEDURE_NAME AS procedure , t.treat_date
                        FROM (SELECT t.TREAT_ID, td.DISEASE_ID , t.treat_date FROM TREATMENT t LEFT JOIN TREAT_DISEASE td ON t.TREAT_ID = td.TREATMENT_ID WHERE t.PATIENT_ID = :patient_id AND td.TREATMENT_ID IS NOT NULL) t 
                        LEFT JOIN NAME_OF_DISEASE nod ON t.DISEASE_ID = nod.DISEASE_ID  
                        LEFT JOIN MEDI_PROCEDURE mp ON nod.MEDI_PROCEDURE_ID = mp.MEDI_PROCEDURE_ID 
                        WHERE mp.DELETE_OR_NOT = 'T') t) tt
                        WHERE rn >= : pre_cnt AND rn < :next_cnt ORDER BY tt.treat_date DESC";
                
                return db.Query<ASModel>(sql, new { patient_id = patient_id , pre_cnt = (cnt <= 0 ? 0 : cnt - 1) * 6, next_cnt = (cnt * 6 + 1)  });
            }
        }
    }
}
