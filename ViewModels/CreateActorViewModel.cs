using System.ComponentModel.DataAnnotations;

namespace Demo.Movies.ViewModels
{
    public class CreateActorViewModel
    {
        [Required(ErrorMessage="The actor's name is required")]
        public string Name { get; set; }
    }
}