using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using test.dto;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository repository;
        private readonly IMapper mapper;
        private readonly IPokemonRepository pokemonRepository;
        private readonly IReviewerRepository reviewerRepository;

        public ReviewController(IReviewRepository repository, IMapper mapper, IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.pokemonRepository = pokemonRepository;
            this.reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = mapper.Map<List<ReviewDTO>>(repository.GetReviews());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("GetReview/{reviewId}")]
        [ProducesResponseType(200,Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewById(int reviewId)
        {
            if(!repository.ReviewExists(reviewId))
                return NotFound();
            
            var review = mapper.Map<ReviewDTO>(repository.GetReview(reviewId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("GetReviewsOfPokemon/{pokeId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfPokemon(int pokeId)
        {
            if(!pokemonRepository.doesPokemonExist(pokeId))
                return NotFound("Pokemon doesnt exist");

            var reviews = mapper.Map<List<ReviewDTO>>(repository.GetReviewsOfPokemon(pokeId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int revieverId, [FromQuery] int pokeId, [FromBody] ReviewDTO reviewDTO)
        {
            if(reviewDTO==null)
                return BadRequest(ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewMap = mapper.Map<Review>(reviewDTO);
            reviewMap.Reviewer = reviewerRepository.GetReviewer(revieverId);
            reviewMap.Pokemon = pokemonRepository.GetPokemonById(pokeId);

            if(!repository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("","Error!");
                return StatusCode(500,ModelState);
            }
            return Ok(reviewMap);

            }
    }
}