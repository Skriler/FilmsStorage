using System.Web.Mvc;

using FilmsStorage.SL;
using FilmsStorage.Filters;


namespace FilmsStorage.Controllers
{
    public class HomeController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        //TODO: Grant "userID = 2" access only
        [RootUserOnly]
        public ViewResult Test()
        {
            return View();
        }

        [Route("RouteTest/a/{a}/b/{b}")]
        public RedirectToRouteResult RouteTest(int a, int b)
        {
            return RedirectToAction("Index");
        }

        public ActionResult ChangeLanguage()
        {
            webSiteLang = (webSiteLang == "uk" ? "en" : "uk");
            _SL.Cookies.SetLanguageCookie(webSiteLang);

            return RedirectToAction("Index");
        }
    }
}