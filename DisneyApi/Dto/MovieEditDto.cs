namespace DisneyApi.Dto
{
    public class MovieEditDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        
        public DateTime Release { get; set; } = new DateTime();

        public int Rating { get; set; } = 1;

      

        public int GenreId { get; set; }

        


    }
}
