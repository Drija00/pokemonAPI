using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using test.dto;
using test.Interfaces;
using test.Models;

namespace test.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory repository;
        private readonly IMapper mapper;

        public CategoryController(ICategory repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = mapper.Map<List<CategoryDTO>>(repository.getCategories());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("api/{categoryId}")]
        [ProducesResponseType(200,Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoryById(int categoryId)
        {
            if(!repository.CategoryExist(categoryId))
            {
                return NotFound();
            }
            var category = mapper.Map<CategoryDTO>(repository.GetCategory(categoryId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpGet("PokemonCategory/{categoryId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)] 
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            if(!repository.CategoryExist(categoryId))
            {
                return NotFound();
            }
            var pokemon = mapper.Map<List<PokemonDTO>>(repository.getPokemonByCategory(categoryId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCategory(int categoryId, CategoryPostDTO categoryDTO)
        {
            if(categoryDTO == null)
                return BadRequest(ModelState);

            if(!repository.CategoryExist(categoryId)){
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var categoryMap = mapper.Map<Category>(categoryDTO);
            categoryMap.Id = categoryId;

            if(!repository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("","Something went wrong!");
                return StatusCode(500,ModelState);
            }
            return Ok("Successfuly updated!");
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromQuery] int pokeId, [FromBody] CategoryPostDTO category)
        {

            if(category==null)
                return BadRequest(ModelState);

            var existing = repository.getCategories().Where(c=>c.Name.Trim().ToUpper()==category.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(existing!=null)
            {
                ModelState.AddModelError("","Category with that name already exist!");
                return StatusCode(403,ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var categoryMap = mapper.Map<Category>(category);

            if(!repository.CreateCategory(pokeId,categoryMap))
            {
                ModelState.AddModelError("","Something went wrong!");
                return StatusCode(500,ModelState);
            }
            return Ok("Successfuly created!");
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult RemoveCategory(int categoryId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var cat = repository.GetCategory(categoryId);
            if(cat==null)
                return NotFound();
            if(!repository.DeleteCategory(cat))
            {
                ModelState.AddModelError("","Error");
                return StatusCode(500,ModelState);
            }
            return Ok("Done!");
        }

    }
}