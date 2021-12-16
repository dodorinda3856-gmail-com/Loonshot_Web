using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;

namespace LoonshotTest.Models.Login
{
    public class LoginModel
    {
        public string patient_login_Id { get; set; }

        public int patient_id { get; set; }

        public string patient_login_pw { get; set; }

        public char status { get; set; }

        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.patient_login_Id);

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.patient_login_Id));

            this.patient_login_pw = System.Convert.ToBase64String(hash);
        }


        public int Register()
        {
            string sql = @"
                      INSERT INTO patient_login(patient_login_id, patient_id, patient_login_pw, status)
                      VALUES(:patient_login_id, null, :patient_login_pw, 'L')";


            using (var db = new MySqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public void UserCheck(string patient_login_id)
        {
            LoginModel loginModel = new LoginModel();

            string sql = @"
                SELECT patient_login_id, patient_id, 
                patient_login_pw, status FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
                loginModel = db.QuerySingle<LoginModel>(sql, this);

                if(loginModel.patient_login_Id == patient_login_id)
                {
                    throw new Exception("이미 사용중인 아이디입니다.");
                }              
            }              
        }

        public LoginModel GetLoginUser()
        {
            LoginModel loginModel = new LoginModel();

            string sql = @"
                SELECT patient_login_id, patient_id, 
                patient_login_pw, status FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
               
                loginModel = db.QuerySingle<LoginModel>(sql, this);
             

                if (loginModel == null)
                {
                    throw new Exception("사용자가 존재하지 않습니다.");
                }

                if (loginModel.patient_login_pw != this.patient_login_pw)
                {
                    throw new Exception("비밀번호가 틀립니다.");
                }
                return loginModel;
            }

        }
        
    }
}
