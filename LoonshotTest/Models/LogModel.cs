using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Models
{
    public class LogModel
    {

        public int log_seq { get; set; }
        public string user_id { get; set; }
        public string log_level { get; set; }
        public string log_massage { get; set; }
        public string log_path { get; set; }
        public string user_ip { get; set; }

        public int logSave()
        {
            using (var db = new MySqlDapperHelper())
            {
                db.BeginTransaction();
                int r = 0;
                try
                {
                    string sql = @"
                        INSERT INTO LOG (LOG_ID, USER_ID, LOG_LEVEL, ACCESS_PATH, LOG_MESSAGE, USER_IP) 
                        VALUES(LOG_SEQ.NEXTVAL, :user_id, :log_level, :log_path, :log_massage, :user_ip)
                    ";
                    
                    r += db.Execute(sql, this);
                    db.Commit();
                    return r;
                }
                catch (Exception ex)
                {
                    db.Rollback();
                }
                return r;
            }
        }
    }
}
