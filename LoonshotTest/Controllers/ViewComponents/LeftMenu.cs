﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest.Controllers.ViewComponents
{
    public class LeftMenu : ViewComponent
    {
        public LeftMenu()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
