using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using test.dto;
using test.Interfaces;
using test.Models;
using test.Repository;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository ownerRepository;
        private readonly IPokemonRepository pokemonRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public OwnerController(IOwnerRepository ownerRepository,IPokemonRepository pokemonRepository,ICountryRepository countryRepository, IMapper mapper)
        {
            this.ownerRepository = ownerRepository;
            this.pokemonRepository = pokemonRepository;
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = mapper.Map<List<OwnerDTO>>(ownerRepository.GetOwners());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }
        [HttpGet("getOwner/{ownerId}")]
        [ProducesResponseType(200,Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerById(int ownerId)
        {
            if(!ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var owner = mapper.Map<OwnerDTO>(ownerRepository.GetOwner(ownerId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        [HttpGet("getOwnersByPokemon/{pokeId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult getOwnersByPokemonId(int pokeId)
        {
            if(!pokemonRepository.doesPokemonExist(pokeId))
                return NotFound("Pokemon doesnt exist");

            var owners = mapper.Map<List<OwnerDTO>>(ownerRepository.GetOwnersOfaPokemon(pokeId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpGet("getPokemonsByOwner/{ownerId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult getPokemonsByOwnerId(int ownerId)
        {
            if(!ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var pokemons = mapper.Map<List<PokemonDTO>>(ownerRepository.GetPokemonsOfaOwner(ownerId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromQuery] int pokeId, [FromBody] OwnerPostDTO ownerDTO)
        {
            if(ownerDTO==null)
                return BadRequest(ModelState);

            var existingOwner = ownerRepository.GetOwners().Where(o=>o.LastName.Trim().ToUpper()==ownerDTO.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if(existingOwner!=null){
                ModelState.AddModelError("","Owner already exist");
                return StatusCode(422,ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            
            var ownerMap = mapper.Map<Owner>(ownerDTO);
            ownerMap.Country = countryRepository.GetCountry(countryId);
        
            if(!ownerRepository.CreateOwner(pokeId,ownerMap))
            {
                ModelState.AddModelError("","Something went wrong");
                return StatusCode(500,ModelState);
            }
            return Ok(ownerMap);
        }


        [HttpPut("{ownerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerPostDTO owner)
        {
            if(owner==null || !ownerRepository.OwnerExists(ownerId) || !ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = mapper.Map<Owner>(owner);
            ownerMap.Id = ownerId;

            if(!ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("","Something went wrong!");
                return StatusCode(500,ModelState);
            }
            return Ok("Ok");
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult RemoveOwner(int ownerId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var o = ownerRepository.GetOwner(ownerId);
            if(o==null)
                return NotFound();
            if(!ownerRepository.DeleteOwner(o))
            {
                ModelState.AddModelError("","Error");
                return StatusCode(500,ModelState);
            }
            return Ok("Done!");
        }
    }
}