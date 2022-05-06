﻿using System;
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

            public static List<Film> ByUser(int userID)
            {
                List<Film> userFilms = new List<Film>();

                using (var db = new FilmsStorageDB())
                {
                    var searchResults = db.Films .Where(f => f.fk_UserID == userID);

                    if (searchResults.Any())
                    {
                        userFilms = searchResults.ToList();
                    }
                }

                return userFilms;
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