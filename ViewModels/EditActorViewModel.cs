using System.Collections.Generic;
using Demo.Movies.Models;

namespace Demo.Movies.ViewModels
{
    public class EditActorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MoviePart> Movies { get; set; }
    }
}