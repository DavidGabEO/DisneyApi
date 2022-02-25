using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DisneyApi.Objects


{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Column(TypeName = "Date")]
        public DateTime Release { get; set; } = new DateTime();

        public int Rating { get; set; } = 1;

        [JsonIgnore]
        public Genre Genre { get; set; }

        public int GenreId { get; set; }

        public List<Character> Characters { get; set; }

    }
}
