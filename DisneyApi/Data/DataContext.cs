using DisneyApi.Objects;
using Microsoft.EntityFrameworkCore;


namespace DisneyApi.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public  DbSet<Genre> Genres { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Character> Characters { get; set; }

    }
}
