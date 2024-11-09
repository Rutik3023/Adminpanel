using  AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AdminPanel.CommonRepo
{
    public class AuthorizeUser : Attribute, IAuthorizationFilter
    {

        public AuthorizeUser()
        {

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {


            if (context.HttpContext.User.Identity.Name != null)
            {

                string user = context.HttpContext.User.Identity.Name;

                var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                var UserCode = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var sessionIdClaim = claimsIdentity?.FindFirst("SessionId")?.Value;

                // Now you can use sessionIdClaim
                string sessionId = sessionIdClaim ?? string.Empty;
                LoginCrediential o = new LoginCrediential();
                o.Id= UserCode;
                o.SessionId= sessionId;
                UserLoginSession ValidCheck= CommonFunction.ActiveLoginCheck(o);
                if (ValidCheck.UserId == null)
                {
                    context.Result = new RedirectToRouteResult(
                 new RouteValueDictionary {
        
        { "controller", "Login" },
        { "action", "Logout" },
        { "id", "" },
        { "flag", "3" },
    }
);
                }
           
                    

            }


        }
    }
}
