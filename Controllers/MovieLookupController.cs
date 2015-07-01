using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Security;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Demo.Movies.Models;
using Demo.Movies.Services;
using Demo.Movies.ViewModels;
using TheMovieDb;

namespace Demo.Movies.Controllers
{
    [Admin]
    public class MovieLookupController : Controller
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IMovieService _movieService;
        private readonly IAuthorizer _authorizer;

        public Localizer T { get; set; }

        public MovieLookupController(IOrchardServices orchardServices, IMovieService movieService, IAuthorizer authorizer)
        {
            _orchardServices = orchardServices;
            _movieService = movieService;
            _authorizer = authorizer;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!_authorizer.Authorize(Permissions.LookupMovie))
            {
                return new HttpUnauthorizedResult();
            }
            return View(new MovieLookupViewModel());
        }

        [HttpPost, ActionName("Index")]
        public ActionResult IndexPost(MovieLookupViewModel viewModel)
        {
            if (!_authorizer.Authorize(Permissions.LookupMovie))
            {
                return new HttpUnauthorizedResult();
            }

            IEnumerable<TmdbMovie> movies;
            var tmdbApi = new TmdbApi(_orchardServices.WorkContext.CurrentSite.As<MovieSettingsPart>().TMDB_ApiKey);

            try
            {
                movies = tmdbApi.MovieSearch(viewModel.MovieTitle);

                if (movies.Any())
                {
                    viewModel.MovieResults = movies.Select(r => new MovieResult { Id = r.Id, Name = r.Name, Released = r.Released });
                }
                else
                {
                    viewModel.NoMatch = true;
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                viewModel.NoMatch = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Import(IEnumerable<int> selectMovieIDs)
        {
            if (!_authorizer.Authorize(Permissions.LookupMovie))
            {
                return new HttpUnauthorizedResult();
            }

            if (selectMovieIDs.Any())
            {
                _movieService.ImportMovies(selectMovieIDs);
                _orchardServices.Notifier.Add(NotifyType.Information, T("Imported the selected movies"));
            }
            return RedirectToAction("Index");
        }
    }
}