using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Demo.Movies.Models;

namespace Demo.Movies.ViewModels
{
    public class MovieLookupViewModel
    {
        public MovieLookupViewModel()
        {
            MovieResults = new List<MovieResult>();
        }

        [Required(ErrorMessage = "The movie title is required")]
        public string MovieTitle { get; set; }
        public string IMDB_Id { get; set; }
        public bool NoMatch { get; set; }
        public IEnumerable<MovieResult> MovieResults { get; set; }
    }
}