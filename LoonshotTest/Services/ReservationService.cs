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
                string sql = "SELECT MEDICAL_STAFF_ID, PATIENT_ID, RESERVATION_DATE, TIME_ID FROM RESERVATION WHERE MEDICAL_STAFF_ID=:doctorId AND TO_CHAR(RESERVATION_DATE, 'YYYY-MM-DD')=:day";
                return db.Query<Reservation>(sql, new { doctorId, day });

            }
        }

        public static ReservationViewModal AddReservation(int patientId, int medicalStaffId, string reservationDate, string symptom, int timeId)
        {
            using (var db = new MySqlDapperHelper())
            {
                string sql = "INSERT INTO RESERVATION (PATIENT_ID, TIME_ID, RESERVATION_DATE, MEDICAL_STAFF_ID, SYMPTOM) VALUES (" +
                    " :patientId, :timeId, To_Date(:reservationDate, 'YYYY-MM-DD'), :medicalStaffId, :symptom)";
                db.Execute(sql, new { patientId, medicalStaffId, reservationDate, timeId, symptom });
                string getMax = "SELECT R.RESERVATION_ID, R.RESERVATION_DATE, R.SYMPTOM, T.HOUR, M.STAFF_NAME, P.PATIENT_NAME " +
                    "FROM(SELECT* FROM RESERVATION WHERE RESERVATION_ID= (SELECT MAX(RESERVATION_ID) FROM RESERVATION)) R " +
                    "INNER JOIN MEDI_STAFF M ON R.MEDICAL_STAFF_ID = M.STAFF_ID INNER JOIN TIME T ON T.TIME_ID = R.TIME_ID " +
                    "INNER JOIN PATIENT P ON P.PATIENT_ID=R.PATIENT_ID";
                return db.QueryFirst<ReservationViewModal>(getMax);
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