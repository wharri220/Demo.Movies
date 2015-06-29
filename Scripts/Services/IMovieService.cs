using Orchard;
using Demo.Movies.Models;
using Demo.Movies.ViewModels;

namespace Demo.Movies.Services
{
    public interface IMovieService : IDependency
    {
        void UpdateMovie(MovieEditViewModel viewModel, MoviePart part);
    }
}
