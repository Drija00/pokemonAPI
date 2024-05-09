using Microsoft.EntityFrameworkCore.Diagnostics;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class CategoryRepository : ICategory{

    private readonly DataContext dataContext;

    public CategoryRepository(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    
        public bool CategoryExist(int id)
        {
            return dataContext.Categories.Any(p=>p.Id == id);
        }

        public bool CreateCategory(int pokeid, Category category)
        {
            var poke = dataContext.Pokemons.Where(p=>p.Id == pokeid).FirstOrDefault();

            var pokecat = new PokemonCategory
            {
                Pokemon = poke,
                Category = category
            };
            dataContext.Add(pokecat);
            dataContext.Add(category); 
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            dataContext.Remove(category);
            return Save();
        }

        public ICollection<Category> getCategories()
        {
            return dataContext.Categories.OrderBy(c=>c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return dataContext.Categories.Where(c=>c.Id==id).FirstOrDefault();
        }

        public ICollection<Pokemon> getPokemonByCategory(int categoryId)
        {
            return dataContext.PokemonCategories.Where(c=>c.CategoryId==categoryId).Select(c=>c.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            dataContext.Update(category);
            return Save();
        }
    }
}
