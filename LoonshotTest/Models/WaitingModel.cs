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
        public int wating_id { get; set; }
        public int patient_Id { get; set; }

        public string patient_login_id { get; set; }

        public string requirements { get; set; }

        public char reserve_status_val { get; set; }
            
        public string request_to_wait { get; set; }

        public int wait_count { get; set; }

        public string alarm_status { get; set; }

        public string phone_num { get; set; }
        public WaitingModel Mywating(WaitingModel waiting)
        {
           using(var db = new MySqlDapperHelper())
            {
                string sql = @"
                    	SELECT wrn.r AS WAIT_COUNT , p.PHONE_NUM , p.patient_id, st AS ALARM_STATUS
	                    FROM (
	                    SELECT
		                    ROWNUM r,
		                    pa,
							st
	                    FROM
		                    (
		                    SELECT
			                    w.REQUEST_TO_WAIT r,
								w.ALARM_STATUS st,
			                    w.PATIENT_ID pa
		                    FROM
			                    WAITING w
		                    WHERE
			                    w.WAIT_STATUS_VAL = 'T'
			                    AND TO_CHAR(w.REQUEST_TO_WAIT , 'YYYY-MM-DD') BETWEEN :request_to_wait AND :request_to_wait
		                    ORDER BY
			                    w.REQUEST_TO_WAIT )
			                    ) wrn LEFT JOIN PATIENT_LOGIN p ON
		                    wrn.pa = p.PATIENT_ID WHERE p.PATIENT_LOGIN_ID  = : patient_login_id and p.DEL_STATUS = 'T'  
                ";
                return db.QuerySingle<WaitingModel>(sql, this);
            }
        }
		public WaitingModel AlarmOff(WaitingModel waiting)
		{
			using (var db = new MySqlDapperHelper())
			{
				string sql = @"
                    	UPDATE WAITING
						SET ALARM_STATUS = 'F'
						WHERE WAIT_STATUS_VAL = 'T' AND patient_id = : patient_id
                ";
				return db.QuerySingle<WaitingModel>(sql, new { patient_id = waiting.patient_Id});
			}
		}
	}
}
