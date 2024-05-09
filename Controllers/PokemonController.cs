using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using test.dto;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository repository;
        private readonly IMapper mapper;
        private readonly IReviewRepository reviewRepository;

        public PokemonController(IPokemonRepository repository, IMapper mapper, IReviewRepository reviewRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = mapper.Map<List<PokemonDTO>>(repository.GetPokemons());

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("getPokemonById/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult getByPokeId(int pokeId)
        {
            if(!repository.doesPokemonExist(pokeId))
            {
                return NotFound();
            }

            var pokemon = mapper.Map<PokemonDTO>(repository.GetPokemonById(pokeId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("getPokemonByName/{name}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult getByName(string name)
        {
            var pokemon = mapper.Map<PokemonDTO>(repository.GetPokemonByName(name));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeid}/rating")]
        [ProducesResponseType(200,Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult getRating(int pokeid)
        {
            if(!repository.doesPokemonExist(pokeid))
            {
                return NotFound();
            }
            var rating = repository.GetPokemonRating(pokeid);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemon)
        {
            if(pokemon==null)
                return BadRequest(ModelState);

            var existingPokemons = repository.GetPokemons().Where(p=>p.Name.Trim().ToUpper()==pokemon.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if(existingPokemons!=null)
            {
                ModelState.AddModelError("","Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = mapper.Map<Pokemon>(pokemon);

            if(!repository.CreatePokemon(ownerId,categoryId,pokemonMap))
            {
                ModelState.AddModelError("","Something went wrong");
                return StatusCode(500,ModelState);
            }
            return Ok(pokemonMap);
        }
        

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePokemon(int pokeId, [FromBody] PokemonPostDTO pokemonDTO)
        {
            if(pokemonDTO==null || !repository.doesPokemonExist(pokeId) || !ModelState.IsValid)
                return BadRequest(ModelState);
            
            var pokeMap = mapper.Map<Pokemon>(pokemonDTO);
            pokeMap.Id = pokeId;
            if(!repository.UpdatePokemon(pokeMap))
            {
                ModelState.AddModelError("","Error!");
                return StatusCode(500,ModelState);
            }
            return Ok("Done!");
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult RemovePokemon(int pokeId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var p = repository.GetPokemonById(pokeId);
            if(p==null)
                return NotFound();
            var reviews = reviewRepository.GetReviewsOfPokemon(pokeId);
            reviewRepository.DeleteReviews(reviews.ToList());
            if(!repository.DeletePokemon(p))
            {
                ModelState.AddModelError("","Error");
                return StatusCode(500,ModelState);
            }
            return Ok("Done!");
        }
    }
}