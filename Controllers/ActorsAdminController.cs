using System.Web.Mvc;
using System.Linq;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Demo.Movies.Models;
using Demo.Movies.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.UI.Admin;
using Orchard.UI.Notify;

namespace Demo.Movies.Controllers
{
    public class ActorsAdminController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly IRepository<ActorRecord> _actorRepository;
        private readonly IOrchardServices _orchardServices;

        public Localizer T { get; set; }
        public dynamic Shape { get; set; }

        public ActorsAdminController(ISiteService siteService, IRepository<ActorRecord> actorRepository, IShapeFactory shapeFactory, IOrchardServices orchardServices)
        {
            _siteService = siteService;
            _actorRepository = actorRepository;
            _orchardServices = orchardServices;
            Shape = shapeFactory;
        }

        [Admin]
        public ActionResult Index(PagerParameters pagerParameters)
        {
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var actorCount = _actorRepository.Table.Count();
            var actors = _actorRepository.Table
                .OrderBy(a => a.Name)
                .Skip((pager.Page - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .ToList();
            var pagerShape = Shape.Pager(pager).TotalItemCount(actorCount);

            var viewModel = new ActorsIndexViewModel(actors, pager);

            return View(viewModel);
        }

        [HttpGet, Admin]
        public ActionResult Create()
        {
            return View(new CreateActorViewModel());
        }

        [HttpPost, ActionName("Create"), Admin]
        public ActionResult CreatePOST(CreateActorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _actorRepository.Create(new ActorRecord { Name = viewModel.Name });
            _orchardServices.Notifier.Add(NotifyType.Information, T("Created the actor {0}", viewModel.Name));
            return RedirectToAction("Index");
        }

        [HttpGet, Admin]
        public ActionResult Edit(int actorId)
        {
            var actor = _actorRepository.Get(actorId);

            if (actor == null)
            {
                return new HttpNotFoundResult("Count not find the actor with id " + actorId);
            }

            var actorMovies = _orchardServices.ContentManager.GetMany<MoviePart>(
                    actor.ActorMovies.Select(m => m.MoviePartRecord.Id), VersionOptions.Published, QueryHints.Empty);
            
            var viewModel = new EditActorViewModel{Id = actorId, Name = actor.Name, Movies = actorMovies};

            return View(viewModel);
        }

        [HttpPost, ActionName("Edit"), Admin]
        public ActionResult EditPOST(EditActorViewModel viewModel)
        {
            var actor = _actorRepository.Get(viewModel.Id);

            if (!ModelState.IsValid)
            {
                viewModel.Movies = _orchardServices.ContentManager.GetMany<MoviePart>(actor.ActorMovies.Select(m => m.Id), VersionOptions.Published, QueryHints.Empty);
                return View("Edit", viewModel);
            }

            actor.Name = viewModel.Name;
            _actorRepository.Update(actor);
            _orchardServices.Notifier.Add(NotifyType.Information, T("Saved {0}", viewModel.Name));
            return RedirectToAction("Index");
        }

        [HttpGet, Admin]
        public ActionResult Delete(int actorId)
        {
            var actor = _actorRepository.Get(actorId);
            if (actor == null)
            {
                return new HttpNotFoundResult("Could not find the actor with Id " + actorId);
            }
            _actorRepository.Delete(actor);
            _orchardServices.Notifier.Add(NotifyType.Information, T("The actor {0} has been deleted", actor.Name));
            return RedirectToAction("Index");
        }
    }
}