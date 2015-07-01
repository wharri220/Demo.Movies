using Orchard.ContentManagement.Records;

namespace Demo.Movies.Models
{
    public class MovieSettingsPartRecord : ContentPartRecord
    {
        public virtual string TMDB_ApiKey { get; set; }
    }
}