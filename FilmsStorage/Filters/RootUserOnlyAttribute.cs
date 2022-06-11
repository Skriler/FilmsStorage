using System.Web.Mvc;
using FilmsStorage.Models.Login;

namespace FilmsStorage.Filters
{
    public class RootUserOnlyAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            RedirectToRouteResult redirectRoute = new RedirectToRouteResult("ShowError", null);

            //TODO: Pass error text to show by /Home/Error

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated || 
                (filterContext.HttpContext.User as CustomPrincipal).UserID != 2)
            {
                filterContext.Result = redirectRoute;
            }
        }
    }
}