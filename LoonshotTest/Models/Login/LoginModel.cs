using MyWeb.Lib.DataBase;
using System;
using System.Diagnostics;

namespace LoonshotTest.Models.Login
{
    public class LoginModel
    {
        public string patient_login_id { get; set; }

        public int patient_id { get; set; }

        public string patient_login_pw { get; set; }

        public char status { get; set; }

        public string phone_num { get; set; }

        public string resident_regist_num { get; set; }

        public string patient_name { get; set; }

        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.patient_login_pw.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.patient_login_pw));

            this.patient_login_pw = System.Convert.ToBase64String(hash);
        }


        public void checkPhone()
        {
            string sql = "SELECT patient_id FROM patient WHERE phone_num = :phone_num";

            using(var db = new MySqlDapperHelper())
            {
                this.patient_id = db.QuerySingle<int>(sql, this);
            }
        }

        public int Register()
        {

            string sql = @"
                      INSERT INTO patient_login(patient_login_id, patient_id, patient_login_pw, status, phone_num, resident_regist_num, patient_name)
                      VALUES(:patient_login_id, :patient_id, :patient_login_pw, :status, :phone_num, :resident_regist_num, :patient_name)";


            using (var db = new MySqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public LoginModel GetUserInfo(string patient_login_id) {
            LoginModel login = new LoginModel();

            string sql = "SELECT * FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
                login = db.QuerySingle<LoginModel>(sql, this);
            }
            return login;
        }


        public int SocialCheck()
        {
            int k = 0;
            LoginModel loginModel;

            string sql = @"
                SELECT patient_login_id FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
                loginModel = db.QuerySingle<LoginModel>(sql, this);
            }

            if (loginModel == null)
            {
                string sql2 = @"
                      INSERT INTO patient_login(patient_login_id, patient_id, patient_login_pw, status, phone_num, patient_name)
                      VALUES(:patient_login_id, :patient_id, :patient_login_pw, :status, :phone_num, :patient_name)";

                using (var db = new MySqlDapperHelper())
                {
                    db.Execute(sql2, this);                    
                }
                k = 1;
                return k;
            }
            else
            {
                k = 2;
                return k;
            }
        }


        public void SocialRegister()
        {
            LoginModel loginModel;
            string sql2 = "SELECT patient_id FROM patient WHERE phone_num = :phone_num";

            using (var db = new MySqlDapperHelper())
            {
                loginModel = db.QuerySingle<LoginModel>(sql2, this);
            }

            if(loginModel != null)
            {
                this.patient_id = loginModel.patient_id;
            }

            string sql = "UPDATE patient_login SET patient_id = :patient_id, phone_num = :phone_num, resident_regist_num = :resident_regist_num, patient_name = :patient_name WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
                db.Execute(sql, this);
            }
        }



        internal LoginModel GetLoginUser()
        {
            LoginModel loginModel;

            string sql = "SELECT * FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {

                loginModel = db.QuerySingle<LoginModel>(sql, this);
            } 
                

            if ( loginModel == null )
            {
                throw new Exception("사용자가 존재하지 않습니다.");
            }

            if ( loginModel.patient_login_pw != this.patient_login_pw)
            {
                throw new Exception("비밀번호가 틀립니다.");
            }

            return loginModel;
            }

        public void UserCheck(string patient_login_id)
        {
            LoginModel loginModel;

            string sql = @"
                SELECT patient_login_id FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
                loginModel = db.QuerySingle<LoginModel>(sql, this);
            }

            if (loginModel != null)
            {
                if (loginModel.patient_login_id == patient_login_id)
                {
                    throw new Exception("이미 사용중인 아이디입니다.");
                }
            }
        }

        public int UserBolt(string patient_login_id)
        {
            using (var db = new MySqlDapperHelper())
            {
                db.BeginTransaction();

                try
                {
                    string sql = @"
                        UPDATE PATIENT_LOGIN
                        SET DEL_STATUS = 'F'
                        WHERE PATIENT_LOGIN_ID = : patient_login_id
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

    }


    }

