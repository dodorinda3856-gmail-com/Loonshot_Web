using LoonshotTest.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoonshotTest
{
    public class Log
    {

        private readonly ILogger<HomeController> _logger;
        public static void test(ILogger<HomeController> logger)
        {
            
            Debug.WriteLine("========================================================================");
            Debug.WriteLine(logger);
            Debug.WriteLine("========================================================================");

        }
    }
}
