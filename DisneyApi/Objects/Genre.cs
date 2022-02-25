

namespace DisneyApi.Objects
{
    public class Genre
    {

        public int Id { get; set; } 

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public List<Movie> Movies { get; set; } 

    }
}
