using test.dto;
using test.Models;

namespace test.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemonById(int id);
        Pokemon GetPokemonByName(string name);
        Pokemon GetPokemonTrimToUpper(PokemonDTO pokemonCreate);
        decimal GetPokemonRating(int pokeid);
        bool doesPokemonExist(int pokeid);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(Pokemon pokemon);
        bool Save();
        bool DeletePokemon(Pokemon pokemon);
    }
}