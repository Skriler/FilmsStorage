using System;
using System.Web.Mvc;
using System.Web.Security;

using FilmsStorage.Models;
using FilmsStorage.DAL;
using FilmsStorage.SL;
using FilmsStorage.Models.Entities;
using FilmsStorage.Mappers;

namespace FilmsStorage.Controllers
{
    public class AccountController : BaseController
    {
        //user profile. access via login
        [Authorize]
        //filter Authorize requires user auth
        //TODO: replace filter to custom
        public ActionResult Index()
        {
            return View();
        }

        #region Login
        //login window
        public ViewResult Login()
        {
            return View(new LoginModel());
        }

        //login handler
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Invalid login form";
                return View();
            }

            //Check if user exists in DB
            User registeredUser = _DAL.Users.ByLogin(loginModel.LoginName);

            //if user does not registered
            if (registeredUser == null)
            {
                ViewBag.ErrorMsg = "No such user";
                return View();
            }

            //Password check
            bool isPasswordValid = _SL.Hasher.checkPassword(registeredUser.Password, loginModel.Password);

            if (!isPasswordValid)
            {
                ViewBag.ErrorMsg = "Wrong password";
                return View();
            }

            _SL.Cookies.SetLoginCookie(registeredUser);

            //Redirect to user profile on login
            return RedirectToAction("Index", "Account");

        }
        #endregion

        #region Logout
        public RedirectToRouteResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
        #endregion

        #region Register
        //Registration window
        public ViewResult Register()
        {
            return View(new RegisterModel());
        }

        //Registration handler
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Invalid register form";
                return View(registerModel);
            }

            //Check if user exists in DB
            User registerUser = _DAL.Users.ByLogin(registerModel.LoginName);

            //If user already exists
            if (registerUser != null)
            {
                ViewBag.ErrorMsg = $"User {registerModel.LoginName} is already registered";

                return View(registerModel);
            }

            User userRegisterEntity = UserMapper.FromRegisterModel(registerModel);
            userRegisterEntity.RegisteredAt = DateTime.Now;

            User registeredUser = _DAL.Users.Register(userRegisterEntity);

            //TODO //create login cookie
            return RedirectToAction("Index", "Account");
        }
        #endregion
    }
}