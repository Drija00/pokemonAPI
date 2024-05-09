using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using test.dto;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository repository;
        private readonly IMapper mapper;

        public ReviewerController(IReviewerRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper= mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = mapper.Map<List<ReviewerDTO>>(repository.GetReviewers());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }
        [HttpGet("GetReviewer/{reviewerId}")]
        [ProducesResponseType(200,Type = typeof(Reviewer))]
        public IActionResult GetReviewerById(int reviewerId)
        {
            if(!repository.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = mapper.Map<ReviewerDTO>(repository.GetReviewer(reviewerId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }

        [HttpGet("GetReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            if(!repository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = mapper.Map<List<ReviewDTO>>(repository.GetReviewsByReviewer(reviewerId));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerPostDTO reviewerDTO)
        {
            if(reviewerDTO==null)
                return BadRequest(ModelState);

            var existingReviewer = repository.GetReviewers().Where(r=>r.Lastname.Trim().ToUpper()==reviewerDTO.Lastname.TrimEnd().ToUpper()).FirstOrDefault();
            if(existingReviewer!=null)
            {
                ModelState.AddModelError("", "This reviewer already exists!");
                return StatusCode(500,ModelState);
            }

            var reviewerMap = mapper.Map<Reviewer>(reviewerDTO);

            if(!repository.CreateReviewer(reviewerMap))
            {
                return BadRequest(ModelState);
            } 
            return Ok(reviewerMap);
        }
    }
}