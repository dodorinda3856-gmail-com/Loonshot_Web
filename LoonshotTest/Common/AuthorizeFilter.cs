using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace LoonshotTest.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class CheckUser : ActionFilterAttribute, IActionFilter
    {
        public CheckUser( )
        {
            Console.WriteLine("");


        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var ctx = context.HttpContext;

            if (ctx.User.Identity == null) {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Action="Login", Controller="Login"}));
            }

            base.OnActionExecuted(context);
        }

    }
}
