using LoonshotTest.Models;
using MyWeb.Lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Services
{
    public class ReservationService
    {
        public static List<Reservation> GetList(int doctorId)
        {
            using (var db = new MySqlDapperHelper())
            {
                var timeStart = DateTime.Now.Date;
                var timeEnd = timeStart.AddDays(6);
                string sql = "SELECT * FROM RESERVATION";
                return db.Query<Reservation>(sql);
            }
        }

        public static void DeleteMyReservation(int patientId, int reservationId)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "DELETE FROM RESERVATION WHERE PATIENT_ID=:patientId AND RESERVATION_ID=:reservationId";
                db.Execute(sql,new { patientId, reservationId });
            }
        }

        public static List<MyReservationRecord> GetMyReservation(int userId)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "SELECT * FROM RESERVATION R JOIN MEDI_STAFF M ON " +
                    "R.MEDICAL_STAFF_ID=M.STAFF_ID WHERE R.RESERVATION_DATE >= TO_DATE('" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', " +
                "'YYYY/MM/DD HH24:MI:SS') AND R.PATIENT_ID=:userId";
;
                return db.Query<MyReservationRecord>(sql, new { userId });
            }
        }

        public static List<Time> GetTimeTableList()
        {
            using (var db = new MySqlDapperHelper())
            {

                string sql = "SELECT * FROM Time";
                return db.Query<Time>(sql);
            }
        }

        public static List<Time> GetTimeTable(int day)
        {
            using (var db = new MySqlDapperHelper())
            {

                string sql = "SELECT * FROM Time WHERE DAY=:day ORDER BY HOUR ASC";
                return db.Query<Time>(sql, new { day });
            }
        }

        public static List<Reservation> GetBookedPatients(string doctorId, string day)
        {
            using (var db = new MySqlDapperHelper())
            {

                string sql = "SELECT  R.MEDICAL_STAFF_ID, R.PATIENT_ID, R.RESERVATION_DATE, R.TIME_ID, T.HOUR FROM (SELECT * FROM RESERVATION WHERE MEDICAL_STAFF_ID=:doctorId AND TO_CHAR(RESERVATION_DATE, 'YYYY-MM-DD')=:day) R INNER JOIN TIME T ON T.TIME_ID=R.TIME_ID";
                return db.Query<Reservation>(sql, new { doctorId, day });

            }
        }

        public static ReservationViewModal AddReservation(int patientId, int medicalStaffId, string reservationDate, string treatType, string symptom, int timeId)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "INSERT INTO RESERVATION (PATIENT_ID, TIME_ID, RESERVATION_DATE, MEDICAL_STAFF_ID, SYMPTOM, TREAT_TYPE) VALUES (" +
                    " :patientId, :timeId, To_Date(:reservationDate, 'YYYY-MM-DD-HH24:MI'), :medicalStaffId, :symptom, :treatType)";
                db.Execute(sql, new { patientId, medicalStaffId, reservationDate, timeId, symptom, treatType });
                string getMax = "SELECT R.RESERVATION_ID, R.RESERVATION_DATE, R.SYMPTOM, T.HOUR, M.STAFF_NAME, P.PATIENT_NAME " +
                    "FROM(SELECT* FROM RESERVATION WHERE RESERVATION_ID= (SELECT MAX(RESERVATION_ID) FROM (SELECT * FROM RESERVATION WHERE PATIENT_ID=:patientId))) R " +
                    "INNER JOIN MEDI_STAFF M ON R.MEDICAL_STAFF_ID = M.STAFF_ID INNER JOIN TIME T ON T.TIME_ID = R.TIME_ID " +
                    "INNER JOIN PATIENT P ON P.PATIENT_ID=R.PATIENT_ID";
                return db.QuerySingle<ReservationViewModal>(getMax, new { patientId});
            }
        }

        public static Time GetTime(int timeId)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "SELECT * FROM TABLE WHERE TIME_ID=:timeId";
                return db.QuerySingle<Time>(sql, new { timeId });
            }
        }

    }
}