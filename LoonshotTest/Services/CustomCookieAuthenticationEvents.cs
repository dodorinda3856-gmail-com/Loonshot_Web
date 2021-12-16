using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    
    
    public CustomCookieAuthenticationEvents()
    {

    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userPrincipal = context.Principal;

        //utc
        var CheckClaim = userPrincipal.Claims.First(p => p.Type == "LastCheckDateTime");
        var lastCheckDateTime = DateTime.ParseExact(CheckClaim.Value, "yyyyMMDDHHmmss", CultureInfo.InvariantCulture);
        int intervalMin = 15;

        if (lastCheckDateTime.AddMinutes(intervalMin) < DateTime.UtcNow)
        {
            //  이 사용자가 정상 사용자인지 검증
            if (1 == 1)
            {
                var identity = userPrincipal.Identity as ClaimsIdentity;
                identity.RemoveClaim(CheckClaim);
                identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMDDHHmmss")));

                await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                

            }
        }
        //  이 사용자가 정상 사용자인지 검증
        else
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
    
}