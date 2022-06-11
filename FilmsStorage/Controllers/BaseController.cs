using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Configuration;
using System.Web;
using System.Linq;

using FilmsStorage.Models.Login;
using FilmsStorage.SL;

namespace FilmsStorage.Controllers
{
    public class BaseController : Controller
    {
        protected CustomPrincipal CurrentUser
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        protected string[] availableCultures = new string[]
        {
            "en", "uk"
        };

        private const string defaultLang = "uk";

        private string _webSiteLang = defaultLang;

        public string webSiteLang
        {
            get
            {
                return _webSiteLang;
            }
            set
            {
                if (availableCultures.Contains(value))
                    _webSiteLang = value;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string langFromRoute = RouteData.Values["lang"]?.ToString();

            if (!string.IsNullOrEmpty(langFromRoute))
            {
                if (availableCultures.Contains(langFromRoute))
                    webSiteLang = langFromRoute;
            }
            else
            {
                HttpCookie langCookie = Request.Cookies["WebSiteLang"];

                if (langCookie == null)
                {
                    string defaultWebSiteLang = ConfigurationManager.AppSettings["DefaultWebSiteLang"];

                    if (!string.IsNullOrEmpty(defaultWebSiteLang))
                    {
                        webSiteLang = defaultWebSiteLang;
                    }

                    _SL.Cookies.SetLanguageCookie(webSiteLang);
                }
                else
                {
                    webSiteLang = langCookie.Value;
                }
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(webSiteLang);
        }
    }
}