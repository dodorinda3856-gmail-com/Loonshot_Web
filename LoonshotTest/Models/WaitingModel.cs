using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LoonshotTest.Models
{
    public class WaitingModel
    {
        public static int rownum = 0;
        
        public int wating_id { get; set; }
        public int patient_Id { get; set; }

        public string requirements { get; set; }

        public char reserve_status_val { get; set; }
            
        public string request_to_wait { get; set; }

        public int Mywating(WaitingModel waiting)
        {
           using(var db = new MySqlDapperHelper())
            {
                string sql = @"
                    	SELECT wrn.r
	                    FROM (
	                    SELECT
		                    ROWNUM r,
		                    pa
	                    FROM
		                    (
		                    SELECT
			                    w.REQUEST_TO_WAIT r,
			                    w.PATIENT_ID pa
		                    FROM
			                    WAITING w
		                    WHERE
			                    w.WAIT_STATUS_VAL = 'T'
			                    AND TO_CHAR(w.REQUEST_TO_WAIT , 'YYYY-MM-DD') BETWEEN :request_to_wait AND :request_to_wait
		                    ORDER BY
			                    w.REQUEST_TO_WAIT )
			                    ) wrn LEFT JOIN PATIENT p ON
		                    wrn.pa = p.PATIENT_ID WHERE p.PATIENT_ID  = : patient_id and p.PATIENT_STATUS_VAL = 'T'  
                ";
                return db.QuerySingle<int>(sql, this);
            }
        }
    }
}
