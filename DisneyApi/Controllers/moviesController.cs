using DisneyApi.Data;
using DisneyApi.Dto;
using DisneyApi.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisneyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class moviesController : ControllerBase
    {

        private readonly DataContext _context;

        public moviesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]    //Listar peliculas con imagen, titulo y fecha
        public async Task<ActionResult<List<Movie>>> Get()
        {
            

            var movies = await _context.Movies.Select(m => new { Image = m.ImageUrl, Title = m.Title, Release = m.Release }).ToListAsync();

            return Ok(movies); 
        }


        [HttpPost] //Agregar pelicula
        public async Task<ActionResult<List<Movie>>>AddMovie(AddMovieDto request)
        {

            if (request.Title== String.Empty ) {
                return BadRequest("Title is required");
            }

            if (request.ImageUrl == String.Empty) {
                return BadRequest("Url to image is required");
            }

            if (request.Rating <= 0 || request.Rating > 5)
            {
                return BadRequest("Wrong rating value");

            }




            var dbGenre = await _context.Genres.FindAsync(request.GenreId);

            if (dbGenre == null)
            {
                return BadRequest("Genre not found");
            }

            var newMovie = new Movie
            {
                Title = request.Title,
                Release = request.Release,
                ImageUrl = request.ImageUrl,
                Rating = request.Rating,
                GenreId = request.GenreId
            };

            newMovie.Release = request.Release;

            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync();
            
            return Ok(await _context.Movies.ToListAsync());
        }


        [HttpPut]
        public async Task<ActionResult<Movie>> UpdateMovie(MovieEditDto request)
        {
            
            var dbMovie = await _context.Movies.FindAsync(request.Id);
            if (dbMovie == null)
            {
                return BadRequest("Movie not found");
            }

            if (request.Title == String.Empty)
            {
                return BadRequest("Title is required");
            }

            if (request.ImageUrl == String.Empty)
            {
                return BadRequest("Url to image is required");
            }

            if (request.Rating <= 0 || request.Rating > 5)
            {
                return BadRequest("Wrong rating value");

            }

            var dbGenre = await _context.Genres.FindAsync(request.GenreId);

            if (dbGenre == null)
            {
                return BadRequest("Genre not found");
            }



            dbMovie.Title = request.Title;
            dbMovie.Release = request.Release;
            dbMovie.ImageUrl = request.ImageUrl;
            dbMovie.Rating = request.Rating;
            dbMovie.GenreId = request.GenreId;

            await _context.SaveChangesAsync();

            return Ok(dbMovie);


        }

        [HttpDelete] // Borrar pelicula
        public async Task<ActionResult<List<Movie>>> Delete(int id)
        {
            var dbMovie = await _context.Movies.FindAsync(id);
            if (dbMovie == null)
            {
                return BadRequest("Character not found.");
            }

            _context.Movies.Remove(dbMovie);
            await _context.SaveChangesAsync();

            return Ok(await _context.Movies.ToListAsync());
        }


        [HttpPost("AddCharacter")] //Agregar personaje a pelicula

        public async Task<ActionResult<Movie>> AddCharacter(AddCharacterToMovieDto request)
        {

            var movie = await _context.Movies.Where(m => m.Id == request.MovieId).Include(m => m.Characters).FirstOrDefaultAsync();

            if(movie == null)
            {
                return BadRequest();
            }

            var character = await _context.Characters.FindAsync(request.CharacterId);

            if(character == null) 
            { 
                return BadRequest(); 
            }

            movie.Characters.Add(character);

            await _context.SaveChangesAsync();

            return movie;


        }

        [HttpDelete("RemoveCharacter")] //Agregar personaje a pelicula

        public async Task<ActionResult<Movie>> RemoveCharacter(AddCharacterToMovieDto request)
        {

            var movie = await _context.Movies.Where(m => m.Id == request.MovieId).Include(m => m.Characters).FirstOrDefaultAsync();

            if (movie == null)
            {
                return BadRequest();
            }

            var character = await _context.Characters.FindAsync(request.CharacterId);

            if (character == null)
            {
                return BadRequest();
            }

            movie.Characters.Remove(character);

            await _context.SaveChangesAsync();

            return movie;


        }



        [HttpGet("name")] //Listar peliculas por nombre
           public async Task<ActionResult<List<Movie>>> GetMovieByName(string name)
           {


               var movies = await _context.Movies.Where(m => m.Title == name).Include(c => c.Characters)
                   .ToListAsync();

               return Ok(movies);


           }



        [HttpGet("genre")] //Listar peliculas por genero
        public async Task<ActionResult<List<Movie>>> GetMovieByGenre(int IdGenre)
        {

            

            var movies = await _context.Movies.Where(m => m.GenreId == IdGenre).Include(c => c.Characters)
                .ToListAsync();
      
            return Ok(movies);

            

        }



        [HttpGet("listgenres")]
        public async Task<ActionResult<List<Genre>>> GetGenreList()    
        {


            return Ok(await _context.Genres.Select(g =>  new {Id = g.Id, Name = g.Name, ImageUrl = g.ImageUrl}).ToListAsync());

          
        }




        [HttpPost("addgenre")] //Agregar Genero

        public async Task<ActionResult<List<Genre>>> AddGenre(GenreDto request)
        {
            if (request.Name == String.Empty)
            {
                return BadRequest("Name is required");
            }

            if (request.ImageUrl == String.Empty)
            {
                return BadRequest("imageUrl is required");
            }

            var newGenre = new Genre
            {
                Name = request.Name,
                ImageUrl = request.ImageUrl,
            };

            _context.Genres.Add(newGenre);

            await _context.SaveChangesAsync();

            return Ok(await _context.Genres.Select(g => new { Id = g.Id, Name = g.Name, ImageUrl = g.ImageUrl }).ToListAsync());


        }



        [HttpPut("editgenre")] //Agregar Genero

        public async Task<ActionResult<List<Genre>>> EditGenre(GenreEditDto request)
        {
            var dbGenre = await _context.Genres.FindAsync(request.Id);
            if (dbGenre == null)
            {
                return BadRequest("Hero not found.");

            }


            if (request.Name == String.Empty)
            {
                return BadRequest("Name is required");
            }

            if (request.ImageUrl == String.Empty)
            {
                return BadRequest("imageUrl is required");
            }

            dbGenre.Name = request.Name;
            dbGenre.ImageUrl = request.ImageUrl;

            
            

            await _context.SaveChangesAsync();

            return Ok(await _context.Genres.Select(g => new { Id = g.Id, Name = g.Name, ImageUrl = g.ImageUrl }).ToListAsync());


        }


        [HttpDelete("deletegenre")] //Agregar Genero

        public async Task<ActionResult<List<Genre>>> DeleteGenre(int id)
        {
            
            var dbGenre = await _context.Genres.FindAsync(id);
            
            var relation = await _context.Movies.Where(x => x.GenreId == id).ToListAsync();

            if (relation.Count != 0)
            {
                
                return BadRequest("Gender associated with movie,cannot be removed ");

            }

            _context.Genres.Remove(dbGenre);
            await _context.SaveChangesAsync();

            return Ok(await _context.Genres.Select(g => new { Id = g.Id, Name = g.Name, ImageUrl = g.ImageUrl }).ToListAsync());

        }


        }
}
