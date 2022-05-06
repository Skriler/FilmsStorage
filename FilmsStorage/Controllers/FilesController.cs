using System;
using System.Collections.Generic;
using System.Web.Mvc;

using FilmsStorage.Models;
using FilmsStorage.DAL;
using FilmsStorage.SL;
using FilmsStorage.Models.Entities;

namespace FilmsStorage.Controllers
{
    //TODO: Insure user is logged in
    [Authorize]
    public class FilesController : BaseController
    {
        // GET: Files By User
        public ActionResult FilmsByUser()
        {
            //TODO: Get Files Of Given User
            List<v_Film> userFilms = _DAL.Films.ByUser(CurrentUser.UserID);

            return PartialView(userFilms);
        }

        #region Add
        //Show add file form
        public ViewResult Add()
        {
            FilmAddModel addFilmModel = new FilmAddModel();

            addFilmModel.ReleaseYear = DateTime.Now.Year;
            addFilmModel.Genres = _DAL.Genres.All();

            return View(addFilmModel);
        }

        [HttpPost]
        public ActionResult Add(FilmAddModel addFilmModel)
        {
            addFilmModel.Genres = _DAL.Genres.All();

            // If film was not posted
            if (!(Request.Files[0].ContentLength > 0))
            {
                ViewBag.ErrorMsg = "No file posted";

                return View(addFilmModel);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Invalid register form";

                return View(addFilmModel);
            }

            FileSaveResult fileInfo = _SL.Files.SaveFilm(Request.Files[0], CurrentUser.UserID);

            //If film was not saved in file system
            if (!fileInfo.IsSaved)
            {
                return View(addFilmModel);
            }

            addFilmModel.UserID = CurrentUser.UserID;
            addFilmModel.FilePath = fileInfo.FilePath;
            addFilmModel.FileName = fileInfo.FileName;

            Film addedFilm = _DAL.Films.Add(addFilmModel);

            //If film was not added in DB
            if (!(addedFilm.FilmID > 0))
            {
                _SL.Files.DeleteFilm(fileInfo.FilePath, fileInfo.FileName);

                ViewBag.ErrorMsg = "File was not added";

                return View(addFilmModel);
            }

            return RedirectToAction("Index", "Account");
        }
        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            var filmByID = _DAL.Films.FilmByID(id);

            if (filmByID == null)
            {
                TempData["Error"] = "No such film";

                return RedirectToAction("Index", "Account");
            }

            //Check if file belongs to current user
            if (filmByID.fk_UserID != CurrentUser.UserID)
            {
                TempData["Error"] = "You do not have permission to interact with this file";

                return RedirectToAction("Index", "Account");
            }

            ViewData["Genres"] = _DAL.Genres.All();

            return View(filmByID);
        }

        [HttpPost]
        public ActionResult Edit(Film updatedFilm)
        {
            ViewData["Genres"] = _DAL.Genres.All();

            //Check if file belongs to current user
            if (updatedFilm.fk_UserID != CurrentUser.UserID)
            {
                TempData["Error"] = "You do not have permission to interact with this file";

                return RedirectToAction("Index", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Invalid edit film form";

                return View(updatedFilm);
            }

            Film editedFilm = _DAL.Films.Edit(updatedFilm);

            if (editedFilm == null)
            {
                ViewBag.ErrorMsg = "File was not edited";

                return View(updatedFilm);
            }

            return RedirectToAction("Index", "Account");
        }
        #endregion

        public ActionResult Details(int id)
        {
            var filmByID = _DAL.Films.ByID(id);

            if (filmByID == null)
            {
                TempData["Error"] = "No such film";

                return RedirectToAction("Index", "Account");
            }

            //Check if file belongs to current user
            if (filmByID.UserID != CurrentUser.UserID)
            {
                TempData["Error"] = "You do not have permission to interact with this file";

                return RedirectToAction("Index", "Account");
            }

            return View(filmByID);
        }

        public RedirectToRouteResult Delete(int id)
        {
            Film filmByID = _DAL.Films.FilmByID(id);
            
            if (filmByID == null)
            {
                TempData["Error"] = "No such file to delete";

                return RedirectToAction("Index", "Account");
            }

            //Check if file belongs to current user
            if (filmByID.fk_UserID != CurrentUser.UserID)
            {
                TempData["Error"] = "You do not have permission to interact with this file";

                return RedirectToAction("Index", "Account");
            }

            Film deletedFilm = _DAL.Films.Delete(id);
            bool isFilmDeleted = _SL.Files.DeleteFilm(deletedFilm.FilePath, deletedFilm.FileName);

            if (!isFilmDeleted)
            {
                TempData["Error"] = "Error deleting film file from file system";
            }

            return RedirectToAction("Index", "Account");
        }
    }
}