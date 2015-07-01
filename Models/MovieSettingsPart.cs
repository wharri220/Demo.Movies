using Orchard.ContentManagement;

namespace Demo.Movies.Models
{
    public class MovieSettingsPart : ContentPart<MovieSettingsPartRecord>
    {
        public string TMDB_ApiKey
        {
            get { return Record.TMDB_ApiKey; }
            set { Record.TMDB_ApiKey = value; }
        }
    }
}