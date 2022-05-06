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
        public ActionResult ByUser()
        {
            //TODO: Get Files Of Given User
            List<Film> userFilms = _DAL.Films.ByUser(CurrentUser.UserID);

            return PartialView();
        }

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
                _SL.Files.DeleteFilm(fileInfo);

                ViewBag.ErrorMsg = "File was not added";

                return View(addFilmModel);
            }

            return RedirectToAction("Index", "Account");
        }
    }
}