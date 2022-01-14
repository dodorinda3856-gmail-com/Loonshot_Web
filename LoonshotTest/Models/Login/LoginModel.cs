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

        public string social_id { get; set; }

        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.patient_login_pw.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.patient_login_pw));

            this.patient_login_pw = System.Convert.ToBase64String(hash);
        }


        public void checkPhone()
        {
            string sql = "SELECT patient_id FROM patient WHERE phone_num = :phone_num AND resident_regist_num = :resident_regist_num";

            using(var db = new MySqlDapperHelper())
            {
                this.patient_id = db.QuerySingle<int>(sql, this);
                
            }
            
            //patient 테이블에 정보가 없는 경우 -> patient 테이블에 데이터 추가 후 patient_id를 생성하여 가져온다.
            if(this.patient_id == 0)
            {
                string sql4 = "SELECT PATIENT_SEQ.NEXTVAL AS patient FROM DUAL";               
                string sql3 = "SELECT PATIENT_SEQ.CURRVAL AS patient FROM DUAL";
                string sql2 = @"
                        INSERT INTO patient(patient_id, resident_regist_num, 
                         phone_num, regist_date, patient_status_val, agree_of_alarm, patient_name)
                        VALUES(:patient_id, :resident_regist_num, :phone_num, CURRENT_TIMESTAMP,
                        'T', 'T', :patient_name) ";
                                   
                using(var db = new MySqlDapperHelper())
                {
                    int k = 0;
                    int j = 0;
                    k = db.QuerySingle<int>(sql4, this);
                    Debug.WriteLine("넥스트시퀀스:" + k);
                    j = db.QuerySingle<int>(sql3, this);

                    Debug.WriteLine("현재시퀀스:" + j);
                    this.patient_id = j;
                    db.Execute(sql2, this);
                }
                 
            } 
        }

        public int Register()
        {

            string sql = @"
                      INSERT INTO patient_login(patient_login_id, patient_id, patient_login_pw, status, phone_num, resident_regist_num, patient_name, del_status)
                      VALUES(:patient_login_id, :patient_id, :patient_login_pw, :status, :phone_num, :resident_regist_num, :patient_name, 'T')";

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

                //최초 로그인의 경우..Patient테이블에 값을 추가해 준다...
                string sql5 = "SELECT PATIENT_SEQ.NEXTVAL AS patient FROM DUAL";
                string sql4 = "SELECT PATIENT_SEQ.CURRVAL AS patient FROM DUAL";
                string sql3 = @"
                        INSERT INTO patient(patient_id, resident_regist_num, patient_name
                         phone_num, regist_date, patient_status_val, agree_of_alarm)
                        VALUES(:patient_id, :resident_regist_num, :patient_name ,:phone_num, CURRENT_TIMESTAMP,
                        'T', 'T') ";

                using (var db = new MySqlDapperHelper())
                {
                    int y = 0;
                    int j = 0;
                    y = db.QuerySingle<int>(sql5, this);
                    Debug.WriteLine("128번 라인의 넥스트시퀀스:" + y);
                    j = db.QuerySingle<int>(sql4, this);
                    Debug.WriteLine("현재시퀀스:" + j);
                    this.patient_id = j;
                    db.Execute(sql3, this);                  
                }

                string sql2 = @"
                      INSERT INTO patient_login(patient_login_id, patient_id, patient_login_pw, status, phone_num, patient_name, del_status)
                      VALUES(:patient_login_id, :patient_id, :patient_login_pw, :status, :phone_num, :patient_name, 'T')";

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
            string sql2 = "SELECT patient_id FROM patient_login WHERE patient_login_id = :patient_login_id";

            using (var db = new MySqlDapperHelper())
            {
                loginModel = db.QuerySingle<LoginModel>(sql2, this);
            }
            
            if(loginModel != null)
            {
                this.patient_id = loginModel.patient_id;
                Debug.WriteLine("167번 라인의 페이션트아이디+++++++++++++++++++++:"+this.patient_id);
            }

            string sql = "UPDATE patient_login SET phone_num = :phone_num, resident_regist_num = :resident_regist_num, patient_name = :patient_name WHERE patient_login_id = :patient_login_id";
            string sql3 = "UPDATE patient SET resident_regist_num = :resident_regist_num, patient_name = :patient_name, phone_num = :phone_num WHERE patient_id = :patient_id";
            using (var db = new MySqlDapperHelper())
            {
                db.Execute(sql, this);
                db.Execute(sql3, this);
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
        public int UserAlarm(LoginModel param , string alarmStatus)
        {
            using (var db = new MySqlDapperHelper())
            {
                db.BeginTransaction();
                try
                {
                    string sql = @"
                        UPDATE PATIENT_LOGIN
						SET ALARM_STATUS = : alarmStatus
						WHERE patient_login_id = : patient_login_id
                    ";
                    int r = 0;
                    r = db.Execute(sql, new {alarmStatus = alarmStatus , patient_login_id = param.patient_login_id });
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

