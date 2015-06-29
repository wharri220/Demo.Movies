using Demo.Movies.Models;
using System.Collections.Generic;

namespace Demo.Movies.ViewModels
{
    public class ActorViewModel
    {
        public string Name { get; set; }
        public IList<MoviePart> Movies { get; set; }
    }
}