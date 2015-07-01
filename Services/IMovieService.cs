using Orchard;
using Demo.Movies.Models;
using Demo.Movies.ViewModels;
using System.Collections.Generic;

namespace Demo.Movies.Services
{
    public interface IMovieService : IDependency
    {
        void UpdateMovie(MovieEditViewModel viewModel, MoviePart part);
        void ImportMovies(IEnumerable<int> tmdbMovieIds);
    }
}
