using System.Collections.Generic;
using Demo.Movies.Models;


namespace Demo.Movies.ViewModels
{
    public class ActorsIndexViewModel
    {
        public ActorsIndexViewModel(IList<ActorRecord> actorList, dynamic pager){
            Actors = actorList;
            Pager = pager;
        }

        public IEnumerable<ActorRecord> Actors { get; set; }
        public dynamic Pager { get; set; }
    }
}