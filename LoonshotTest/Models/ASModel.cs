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
        public int disease_id { get; set; }
        
        public string disease_name { get; set; }
        public string d_as { get; set; }
        public int medi_procedure_id { get; set; }
        public string procedure_name { get; set; }
        public string m_as { get; set; }

        public static List<ASModel> UserAS(int patient_id)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = @"
                    	SELECT nod.DISEASE_ID , nod.DISEASE_NAME, nod.A_S as D_AS , mp.MEDI_PROCEDURE_ID , mp.PROCEDURE_NAME , mp.A_S AS M_AS
                        FROM(SELECT t.TREAT_ID, td.DISEASE_ID FROM TREATMENT t LEFT JOIN TREAT_DISEASE td ON t.TREAT_ID = td.TREATMENT_ID WHERE t.PATIENT_ID = : patient_id AND t.TREAT_STATUS__VAL = 'T' AND td.TREATMENT_ID IS NOT NULL) t
                        LEFT JOIN NAME_OF_DISEASE nod ON t.DISEASE_ID = nod.DISEASE_ID
                        LEFT JOIN MEDI_PROCEDURE mp ON nod.MEDI_PROCEDURE_ID = mp.MEDI_PROCEDURE_ID
                        WHERE nod.DELETE_OR_NOT = 'T' AND mp.DELETE_OR_NOT = 'T'
                ";
                return db.Query<ASModel>(sql, new { patient_id = patient_id });
            }
        }
    }
}
