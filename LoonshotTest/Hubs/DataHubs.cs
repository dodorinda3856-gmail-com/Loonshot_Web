using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using LoonshotTest.Models;
using CoolSms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoonshotTest.Hubs;
using System.Threading;

namespace LoonshotTest.Hubs
{
    public class DataHubs : Hub
    {
        public int Send(string patient_id)
        {
            return GetMyWaiting(patient_id);
        }

        [HttpGet]
        public int GetMyWaiting(string p_id)
        {
            WaitingModel waitmodel = new WaitingModel();
            try
            {
                waitmodel.patient_login_id = p_id;
                waitmodel.request_to_wait = DateTime.Now.ToString("yyyy-MM-dd");
                waitmodel = waitmodel.Mywating(waitmodel);

                if (waitmodel.alarm_status == "T" && waitmodel.wait_count == 3)
                {
                    SendLMS.Run(waitmodel.phone_num);
                    waitmodel.AlarmOff(waitmodel);
                }
                return (waitmodel.wait_count);
            }
            catch (Exception ex)
            {
                Log.ERROR(ex, "login");
                return (0);
            }
        }
    }
}