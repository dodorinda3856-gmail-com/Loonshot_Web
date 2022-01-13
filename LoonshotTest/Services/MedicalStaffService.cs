using LoonshotTest.Models;
using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Services
{
    public class MediStaffService
    {
        public static List<MediStaff> GetList()
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "SELECT * FROM MedI_STAFF WHERE POSITION='D'";
                return db.Query<MediStaff>(sql);
            }
        }

        public static MediStaff GetDoctorInfo(int id)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "SELECT * FROM MEDI_STAFF WHERE STAFF_ID=:id AND POSITION='D'";
                return db.QuerySingle<MediStaff>(sql, new { id });
            }
        }


        
    }
}
