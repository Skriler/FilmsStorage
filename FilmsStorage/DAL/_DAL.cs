using System;
using System.Collections.Generic;
using System.Linq;

using FilmsStorage.Mappers;
using FilmsStorage.Models;
using FilmsStorage.Models.Entities;

namespace FilmsStorage.DAL
{
    public static class _DAL
    {
        public static class Users
        {
            public static User ByLogin(string loginName)
            {
                using (var db = new FilmsStorageDB())
                {
                    User registeredUser = null;

                    //TODO: rework without checking on exception
                    try
                    {
                        registeredUser = db.Users.Where(u => u.Login == loginName).First();
                    }
                    catch (InvalidOperationException) { }

                    return registeredUser;
                }
            }

            public static User Register(User registerModel)
            {
                using (var db = new FilmsStorageDB())
                {
                    db.Users.Add(registerModel);
                    db.SaveChanges();
                }

                return registerModel;
            }

        }

        public static class Films 
        {
            public static Film Add(FilmAddModel addFilmModel)
            {
                using (var db = new FilmsStorageDB())
                {
                    Film filmToAdd = FilmMapper.FromAddModel(addFilmModel);

                    db.Films.Add(filmToAdd);
                    db.SaveChanges();

                    return filmToAdd;
                }
            }

            public static Film Edit(Film editFilmModel)
            {
                using (var db = new FilmsStorageDB())
                {
                    Film filmToEdit = db.Films.Where(f => f.FilmID == editFilmModel.FilmID).First();

                    if (filmToEdit != null)
                    {
                        filmToEdit.FilmName = editFilmModel.FilmName;
                        filmToEdit.ReleaseYear = editFilmModel.ReleaseYear;
                        filmToEdit.fk_GenreID = editFilmModel.fk_GenreID;
                        filmToEdit.FilmDescription = editFilmModel.FilmDescription;

                        db.SaveChanges();
                    }

                    return filmToEdit;
                }
            }

            public static Film Delete(int filmID)
            {
                Film deletedFile = null;

                using (var db = new FilmsStorageDB())
                {
                    var searchResuls = db.Films.Where(f => f.FilmID == filmID);

                    if (searchResuls.Any())
                    {
                        Film filmToDelete = searchResuls.First();

                        deletedFile = db.Films.Remove(filmToDelete);
                        db.SaveChanges();
                    }
                }

                return deletedFile;
            }

            public static List<v_Film> ByUser(int userID)
            {
                List<v_Film> userFilms = new List<v_Film>();

                using (var db = new FilmsStorageDB())
                {
                    var searchResults = db.v_Films.Where(f => f.UserID == userID);

                    if (searchResults.Any())
                    {
                        userFilms = searchResults.ToList();
                    }
                }

                return userFilms;
            }

            public static Film FilmByID(int filmID)
            {
                Film filmByID = null;

                using (var db = new FilmsStorageDB())
                {
                    var searchResults = db.Films.Where(f => f.FilmID == filmID);

                    if (searchResults.Any())
                    {
                        filmByID = searchResults.First();
                    }
                }

                return filmByID;
            }

            public static v_Film ByID(int filmID)
            {
                v_Film filmByID = null;

                using (var db = new FilmsStorageDB())
                {
                    //Turn LazyLoading off to allow JSON serialization
                    //DB Record will be returned immediately
                    db.Configuration.LazyLoadingEnabled = false;

                    var searchResults = db.v_Films.Where(f => f.FilmID == filmID);

                    if (searchResults.Any())
                    {
                        filmByID = searchResults.First();
                    }
                }

                return filmByID;
            }
        }

        public static class Genres 
        {
            public static List<Genre> All()
            {
                using (var db = new FilmsStorageDB())
                {
                    return db.Genres.ToList();
                }
            }
        }
    }
}