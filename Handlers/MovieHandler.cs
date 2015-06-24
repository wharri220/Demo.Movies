using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Demo.Movies.Models;

namespace Demo.Movies.Handlers
{
    public class MovieHandler : ContentHandler
    {
        public MovieHandler(IRepository<MoviePartRecord> moviePartRepository)
        {
            Filters.Add(StorageFilter.For(moviePartRepository));
        }

    }
}