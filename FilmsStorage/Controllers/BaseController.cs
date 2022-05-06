using System.Web.Mvc;

using FilmsStorage.Models.Login;

namespace FilmsStorage.Controllers
{
    public class BaseController : Controller
    {
        protected CustomPrincipal CurrentUser
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}