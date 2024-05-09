using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using test.dto;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository repository;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Country>))]
        public IActionResult getCountries()
        {
            var countries = mapper.Map<List<CountryDTO>>(repository.GetCountries());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet("getCountryById/{countryId}")]
        [ProducesResponseType(200,Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult getCountryById(int countryId)
        {
            if(!repository.CountryExist(countryId))
                return NotFound();

            var country = mapper.Map<CountryDTO>(repository.GetCountry(countryId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("getCountryByOwnerId/{ownerId}")]
        [ProducesResponseType(200,Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult getCountryByOwnerId(int ownerId)
        {
            //fali provera da li owner postoji
            var country = mapper.Map<CountryDTO>(repository.GetCountryByOwner(ownerId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult getOwnersByCountryId(int countryId)
        {
            if(!repository.CountryExist(countryId))
                return NotFound();
            
            var owners = mapper.Map<List<Owner>>(repository.GetOwnersByCountry(countryId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCountry(int countryId, CountryPostDTO country)
        {
            if(country==null || !repository.CountryExist(countryId) || !ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = mapper.Map<Country>(country);
            countryMap.Id = countryId;
            if(!repository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("","Something went wrong!");
                return StatusCode(500,ModelState);
            }
            return Ok("Updated succesfuly!");
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult RemoveCountry(int countryId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var c= repository.GetCountry(countryId);
            if(c==null)
                return NotFound();
            if(!repository.DeleteCountry(c))
            {
                ModelState.AddModelError("","Error");
                return StatusCode(500,ModelState);
            }
            return Ok("Done!");
        }
    }
}