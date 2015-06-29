using Orchard.Data;
using System.Linq;
using System.Collections.Generic;
using Demo.Movies.Models;
using Demo.Movies.ViewModels;

namespace Demo.Movies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<ActorRecord> _actorRepository;
        private readonly IRepository<MovieActorRecord> _movieActorRepository;
        private readonly IRepository<MoviePartRecord> _moviePartRepository;

        public MovieService(IRepository<ActorRecord> actorRepository, IRepository<MovieActorRecord> movieActorReposiory, IRepository<MoviePartRecord> moviePartRepository)
        {
            _actorRepository = actorRepository;
            _movieActorRepository = movieActorReposiory;
            _moviePartRepository = moviePartRepository;
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
    }
}