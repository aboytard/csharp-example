using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlbanWebApplication.Models
{
    public class MvcGenreMovieViewModel
    {
        public List<Movie>? Movies { get; set; }
        public SelectList? Genres { get; set; }
        public string? MovieGenre { get; set; }
        public string? SearchString { get; set; }
    }
}
