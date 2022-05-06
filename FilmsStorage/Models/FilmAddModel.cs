using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using FilmsStorage.Models.Entities;

namespace FilmsStorage.Models
{
    public class FilmAddModel
    {
        [Required]
        public string FilmName { get; set; }

        [Required]
        //TODO: Create custom validator for insure Movie range (1985 - Current Year)
        public int ReleaseYear { get; set; }

        [Required]
        public int GenreID { get; set; }

        public int UserID { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FilmDescription { get; set; }

        public List<Genre> Genres { get; set; }

        //TODO: Extend model with genre list
    }
}