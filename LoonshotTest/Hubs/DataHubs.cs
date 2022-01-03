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
        bool createdNew = false;
        public int Send(string patient_id , string cookieCheck) {

            return GetMyWaiting(patient_id, cookieCheck);
        }

        [HttpGet]
        public int GetMyWaiting(string p_id , string cookie) {
            WaitingModel waitmodel = new WaitingModel();
            try
            {

                waitmodel.patient_login_id = p_id;
                waitmodel.request_to_wait = DateTime.Now.ToString("yyyy-MM-dd");
                waitmodel = waitmodel.Mywating(waitmodel);


                Mutex dup = new Mutex(true, "File Sync Manager", out createdNew);
                if (createdNew)
                {
                    SendLMS.Run(waitmodel.phone_num);
                }

                return (waitmodel.wait_count);

            }
            catch (Exception ex)
            {
                return (0);
            }

        }
    }
}
