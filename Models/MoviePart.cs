using Orchard.ContentManagement;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Movies.Models
{
    public class MoviePart : ContentPart<MoviePartRecord>
    {
        public string IMDB_Id
        {
            get { return Record.IMDB_Id; }
            set { Record.IMDB_Id = value; }
        }

        public int YearReleased
        {
            get { return Record.YearReleased; }
            set { Record.YearReleased = value; }
        }

        public MPAARating Rating
        {
            get { return Record.Rating; }
            set { Record.Rating = value; }
        }

        public IEnumerable<ActorRecord> Cast
        {
            get { return Record.MovieActors.ToList().Select(c => c.ActorRecord); }
        }
    }
}