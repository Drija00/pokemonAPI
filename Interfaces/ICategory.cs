using test.Models;

namespace test.Interfaces
{
    public interface ICategory
    {
        ICollection<Category> getCategories();

        Category GetCategory(int id);

        ICollection<Pokemon> getPokemonByCategory(int categoryId);

        bool UpdateCategory(Category category); 

        bool CreateCategory(int pokeid, Category category);

        bool CategoryExist(int id);

        bool Save();

        bool DeleteCategory(Category category);

    }
}