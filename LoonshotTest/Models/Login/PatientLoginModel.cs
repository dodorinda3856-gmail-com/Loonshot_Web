using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWeb.Lib.DataBase;

namespace LoonshotTest.Models.Login
{
    /*
    public class PatientLoginModel
    {
        
        public string staff_login_Id { get; set; }

        public int staff_id { get; set; }

        public string staff_login_pw { get; set; }

        public char status { get; set; }

        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.staff_login_Id);

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.staff_login_Id));

            this.staff_login_pw = System.Convert.ToBase64String(hash);
        }


        public int Register()
        {
            string sql = @"
                      INSERT INTO MEDI_STAFF_LOGIN(staff_login_id, staff_id, staff_login_pw, status)
                      VALUES(:staff_login_id, :staff_id, :staff_login_pw 'L')";


            using (var db = new MySqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        
        internal LoginModel GetLoginUser(string staff_login_id)
        {
            
            using (var db = new MySqlDapperHelper())
            {
                string sql = @"
                SELECT staff_login_id, staff_id, staff_login_pw FROM medi_staff_login where staff_login_id = :staff_login_id";

            } 

        }
        */
}

