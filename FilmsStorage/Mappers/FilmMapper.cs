using FilmsStorage.Models;
using FilmsStorage.Models.Entities;

namespace FilmsStorage.Mappers
{
    public class FilmMapper
    {
        public static Film FromAddModel(FilmAddModel filmAddModel)
        {
            return new Film()
            {
                FilmName = filmAddModel.FilmName,
                ReleaseYear = filmAddModel.ReleaseYear,
                fk_GenreID = filmAddModel.GenreID,
                fk_UserID = filmAddModel.UserID,
                FileName = filmAddModel.FileName,
                FilePath = filmAddModel.FilePath,
                FilmDescription = filmAddModel.FilmDescription
            };
        }
    }
}