using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmsStorage.Controllers
{
    public class HomeController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}