using LoonshotTest.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using LoonshotTest.Models;
namespace LoonshotTest
{
    public class Log
    {
        static StackTrace st = new StackTrace(true);

        public static string GetIP4() {
            string strIP4 = string.Empty;

            foreach (IPAddress objIP in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (objIP.AddressFamily.ToString() == "InterNetwork")
                {
                    strIP4 = objIP.ToString();
                    break;
                }
            }
            return strIP4;
        }

        #region 로그DEBUG
        public static void Infomation(string  path, string user_id) {
            LogModel logmodel = new LogModel();
            logmodel.user_ip = GetIP4();
            logmodel.log_path = path;
            logmodel.user_id = user_id;
            logmodel.log_massage = "";
            logmodel.log_level = "DEBUG";
            logmodel.logSave();
        }
        #endregion

        #region 로그ERROR
        public static void ERROR(Exception e, string user_id)
        {
            LogModel logmodel = new LogModel();
            logmodel.user_ip = GetIP4();
            logmodel.log_path = e.StackTrace;
            logmodel.user_id = user_id;
            logmodel.log_level = "ERROR";
            logmodel.log_massage = e.Message;
            logmodel.logSave();
        }
        #endregion

        #region 로그FATAL
        public static void FATAL(Exception e, string user_id)
        {
            LogModel logmodel = new LogModel();
            logmodel.user_ip = GetIP4();
            logmodel.log_path = e.StackTrace;
            logmodel.user_id = user_id;
            logmodel.log_level = "FATAL";
            logmodel.log_massage = e.Message;
            logmodel.logSave();
        }
        #endregion

    }
}
