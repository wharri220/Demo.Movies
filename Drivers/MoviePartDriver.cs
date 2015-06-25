using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Demo.Movies.Models;
using Demo.Movies.ViewModels;
using Demo.Movies.Services;
using Orchard.Data;
using System.Linq;

namespace Demo.Movies.Drivers
{
    public class MoviePartDriver : ContentPartDriver<MoviePart>
    {
        private IRepository<ActorRecord> _actorRepository;
        private IMovieService _movieService;

        public MoviePartDriver(IRepository<ActorRecord> actorRepository, IMovieService movieService)
        {
            _actorRepository = actorRepository;
            _movieService = movieService;
        }

        protected override string Prefix
        {
            get { return "Movie"; }
        }

        protected override DriverResult Display(MoviePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Movie", () =>
                shapeHelper.Parts_Movie(MoviePart: part));
        }

        //GET
        protected override DriverResult Editor(MoviePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Movie_Edit", () =>
                shapeHelper.EditorTemplate(TemplateName: "Parts/Movie", Model: BuildEditViewModel(part), Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(MoviePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new MovieEditViewModel();
            updater.TryUpdateModel(viewModel, Prefix, null, new []{"AllActors"});
            _movieService.UpdateMovie(viewModel, part);
            return Editor(part, shapeHelper);
        }

        private MovieEditViewModel BuildEditViewModel(MoviePart part)
        {
            return new MovieEditViewModel
            {
                IMDB_Id = part.IMDB_Id,
                YearReleased = part.YearReleased,
                Rating = part.Rating,
                Actors = part.Cast.Select(c => c.Id).ToList(),
                AllActors = _actorRepository.Table.OrderBy(a => a.Name).ToList()
            };
        }
    }
}