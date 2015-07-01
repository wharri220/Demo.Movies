using Orchard;
using Orchard.Taxonomies;
using Orchard.Taxonomies.Services;
using Orchard.Taxonomies.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Core.Common;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using Demo.Movies.Models;
using Demo.Movies.ViewModels;
using TheMovieDb;

namespace Demo.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<ActorRecord> _actorRepository;
        private readonly IRepository<MovieActorRecord> _movieActorRepository;
        private readonly IRepository<MoviePartRecord> _moviePartRepository;
        private readonly IOrchardServices _orchardServices;
        private readonly ITaxonomyService _taxonomyService;
        private  Lazy<TmdbApi> _tmdbApi;

        public MovieService(IRepository<ActorRecord> actorRepository, IRepository<MovieActorRecord> movieActorReposiory, 
            IRepository<MoviePartRecord> moviePartRepository, IOrchardServices orchardServices, ITaxonomyService taxonomyService)
        {
            _actorRepository = actorRepository;
            _movieActorRepository = movieActorReposiory;
            _moviePartRepository = moviePartRepository;
            _orchardServices = orchardServices;
            _taxonomyService = taxonomyService;
            _tmdbApi = new Lazy<TmdbApi>(() => new TmdbApi(_orchardServices.WorkContext.CurrentSite.As<MovieSettingsPart>().TMDB_ApiKey));
        }

        public void UpdateMovie(MovieEditViewModel model, MoviePart part)
        {
            part.IMDB_Id = model.IMDB_Id;
            part.YearReleased = model.YearReleased;
            part.Rating = model.Rating;

            var oldCast = _movieActorRepository.Fetch(ma => ma.MoviePartRecord.Id == part.Id);

            List<int> oldActors = new List<int>();

            foreach (var actor in oldCast)
            {
                oldActors.Add(actor.ActorRecord.Id);
            }
            
            foreach (var oldActorId in oldActors.Except(model.Actors))
            {
                var actor = _movieActorRepository.Get(r => r.ActorRecord.Id == oldActorId);
                _movieActorRepository.Delete(actor);
            }

            foreach (var newActorId in model.Actors.Except(oldActors))
            {
                var actor = _actorRepository.Get(newActorId);
                var moviePart = _moviePartRepository.Get(part.Id);
                _movieActorRepository.Create(new MovieActorRecord { ActorRecord = actor, MoviePartRecord = moviePart });
            }
        }
        
        public void ImportMovie(int tmdbMovieId)
        {
            try
            {
                var movieInfo = _tmdbApi.Value.GetMovieInfo(tmdbMovieId);
                var movie = _orchardServices.ContentManager.New("Movie");

                movie.As<TitlePart>().Title = movieInfo.Name;
                movie.As<BodyPart>().Text = movieInfo.Overview;
                if (movieInfo.Released.Contains("-"))
                {
                    movie.As<MoviePart>().YearReleased = int.Parse(movieInfo.Released.Split('-')[0]);
                }
                movie.As<MoviePart>().Rating = (MPAARating)Enum.Parse(typeof(MPAARating), movieInfo.Certification);
                movie.As<MoviePart>().IMDB_Id = movieInfo.ImdbId;

                AssignGenres(movie, movieInfo);
                AssignActors(movie.As<MoviePart>(), movieInfo);
                _orchardServices.ContentManager.Create(movie, VersionOptions.Published);
            }
            catch (Exception ex)
            {
                _orchardServices.TransactionManager.Cancel();
                throw;
            }
        }

        public void ImportMovies(IEnumerable<int> tmdbMovieIds)
        {
            foreach (var movieId in tmdbMovieIds)
            {
                ImportMovie(movieId);
            }
        }

        private void AssignGenres(ContentItem movie, TmdbMovie tmdbMovie)
        {
            var genreTaxonomy = _taxonomyService.GetTaxonomyByName("Genre");
            var allGenres = _taxonomyService.GetTerms(genreTaxonomy.Id);
            var movieGenres = new List<TermPart>();

            foreach (var tmdbGenre in tmdbMovie.Genres)
            {
                var genre = allGenres.SingleOrDefault(g => g.Name == tmdbGenre.Name);
                if (genre == null)
                {
                    genre = _taxonomyService.NewTerm(genreTaxonomy);
                    genre.Name = tmdbGenre.Name;
                    _orchardServices.ContentManager.Create(genre, VersionOptions.Published);
                }
                movieGenres.Add(genre);
            }

            _taxonomyService.UpdateTerms(movie, movieGenres, "Genre");
        }

        private void AssignActors(MoviePart movie, TmdbMovie movieInfo)
        {
            var movieActors = new List<MovieActorRecord>();

            foreach (var tmdbCastMember in movieInfo.Cast.Where(c => c.Job == "Actor").OrderBy(c => c.Order).Take(5))
            {
                var actor = _actorRepository.Fetch(a => a.Name == tmdbCastMember.Name).SingleOrDefault();
                if (actor == null)
                {
                    actor = new ActorRecord { Name = tmdbCastMember.Name };
                    _actorRepository.Create(actor);
                }
                movieActors.Add(new MovieActorRecord { ActorRecord = actor, MoviePartRecord = movie.Record });
            }
            movieActors.ForEach(ma => _movieActorRepository.Create(ma));
        }
    }
}