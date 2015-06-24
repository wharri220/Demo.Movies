using Demo.Movies.Models;
using System.Collections.Generic;

namespace Demo.Movies.ViewModels
{
    public class MovieEditViewModel
    {
        public string IMDB_Id { get; set; }
        public int YearReleased { get; set; }
        public MPAARating Rating { get; set; }
        public IEnumerable<int> Actors { get; set; }
        public IEnumerable<ActorRecord> AllActors { get; set; }
    }
}