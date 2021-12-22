using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using LoonshotTest.Interface;
using LoonshotTest.Models;

namespace LoonshotTest.Hubs
{
    public class DataHubs : Hub
    {
        int user_id;

        public int Send(int patient_id) { 
            user_id = patient_id;

            return GetMyWaiting(user_id);
        }

        public int GetMyWaiting(int p_id) {
            WaitingModel waitmodel = new WaitingModel();
            waitmodel.patient_Id = p_id;
            waitmodel.request_to_wait = DateTime.Now.ToString("yyyy-MM-dd");
            int mywait = waitmodel.Mywating(waitmodel);
            return (mywait);

        }
    }
}
