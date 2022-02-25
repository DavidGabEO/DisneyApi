namespace DisneyApi.Dto
{
    public class AddMovieDto
    {
        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

       
        public DateTime Release { get; set; } = new DateTime();

        public int Rating { get; set; } = 1;

        public int GenreId { get; set; } = 1;
    }
}
