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
    public class charactersController : ControllerBase
    {
        private readonly DataContext _context;

        public charactersController(DataContext context)
        {
            _context = context;
        }



        [HttpGet]    //Listar personajes con imagen y nombre
        public async Task<ActionResult<List<Character>>> Get()
        {


         var characters = await _context.Characters.Select(c => new { Image = c.imageUrl, Name = c.Name }).ToListAsync();

         return Ok(characters);
        }


        [HttpPost] // Crear personaje
        public async Task<ActionResult<List<Character>>> AddCharacter(CharacterCreationDto character)
        {

            if (character.imageUrl == String.Empty)
            {
                return BadRequest("imageUrl is required");
            }


            if (character.Name == String.Empty)
            {
                return BadRequest("Name is required");
            }





            if (character.Age < 0)
            {
                return BadRequest("Wrong age value");
            }


            bool NumberCheck = String.IsNullOrEmpty(character.Age.ToString());


            if (NumberCheck)
            {
                return BadRequest("Age is required");
            }



            if (character.Role == String.Empty)
            {
                return BadRequest("Role is required");
            }


            if (character.Story == String.Empty)
            {
                return BadRequest("Story is required");
            }

            Character newCharacter = new Character()
            {
                imageUrl = character.imageUrl,
                Name = character.Name,
                Age = character.Age,
                Role = character.Role,
                Story = character.Story
            };
            
            _context.Characters.Add(newCharacter);

            await _context.SaveChangesAsync();

            return Ok(await _context.Characters.ToListAsync());
        }


        [HttpPut] // Editar Personaje
        public async Task<ActionResult<Character>> UpdateCharacter(CharacterEditDto request)
        {
            var dbCharacter = await _context.Characters.FindAsync(request.Id);
            if (dbCharacter == null)
            {
                return BadRequest("Character not found");
            }



            if (request.imageUrl == String.Empty)
            {
                return BadRequest("imageUrl is required");
            }


            if (request.Name == String.Empty)
            {
                return BadRequest("Name is required");
            }





            if (request.Age < 0)
            {
                return BadRequest("Wrong age value");
            }


            bool NumberCheck = String.IsNullOrEmpty(request.Age.ToString());


            if (NumberCheck)
            {
                return BadRequest("Age is required");
            }



            if (request.Role == String.Empty)
            {
                return BadRequest("Role is required");
            }


            if (request.Story == String.Empty)
            {
                return BadRequest("Story is required");
            }







            dbCharacter.Name = request.Name;
            dbCharacter.imageUrl = request.imageUrl;
            dbCharacter.Age = request.Age;
            dbCharacter.Role = request.Role;
            dbCharacter.Story = request.Story;

            await _context.SaveChangesAsync();

            return Ok(dbCharacter);


        }


        [HttpDelete] //Eliminar personaje

        public async Task<ActionResult<List<Character>>> Delete(int id)
        {
            var dbCharacter = await _context.Characters.FindAsync(id);
            if (dbCharacter == null)
            {
                return BadRequest("Character not found.");
            }

            _context.Characters.Remove(dbCharacter);
            await _context.SaveChangesAsync();

            return Ok(await _context.Characters.ToListAsync());
        }



        [HttpGet("details")]    //Listar personajes con detalles
        public async Task<ActionResult<List<CharacterDto>>> GetCharactersDetails()
        {

            List<CharacterDto> CharacterList = new List<CharacterDto>();

            var characters = await _context.Characters.Include(m => m.Movies).ToListAsync();

            for(int i = 0; i < characters.Count; i++)
            {
                var newCharacter = new CharacterDto();

                newCharacter.Id = characters[i].Id;
                newCharacter.Name = characters[i].Name;
                newCharacter.imageUrl = characters[i].imageUrl;
                newCharacter.Story = characters[i].Story;
                newCharacter.Role = characters[i].Role;
                newCharacter.Age = characters[i].Age;
                newCharacter.Movies = characters[i].Movies;

                

                CharacterList.Add(newCharacter);
                
            }
            


            return Ok(CharacterList);
        }


        [HttpGet("name")] // Buscar personajes por nombre
        public async Task<ActionResult<List<Character>>> GetbyName(string name)
        {


            var characters = await _context.Characters.Where(c => c.Name == name).ToListAsync();

            return Ok(characters);

           


        }

        [HttpGet("age")] //Buscar personajes por edad
        public async Task<ActionResult<List<Character>>> GetbyAge(int age)
        {


            var characters = await _context.Characters.Where(c => c.Age == age).ToListAsync();

            return Ok(characters);




        }


        [HttpGet("movies")] // Buscar personajes por peliculas
        public async Task<ActionResult<List<Character>>> GetbyMovieId(int MovieId)
        {

            var characters = await _context.Movies.Where(m=> m.Id == MovieId).Select(m=> new {Title = m.Title, Characters = m.Characters})
            .ToListAsync();



            return Ok(characters);




        }


    }
}
