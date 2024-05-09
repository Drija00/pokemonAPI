using AutoMapper;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.dto;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class PokemonRepository : IPokemonRepository
    {

        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public PokemonRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokOwnCreate = dataContext.Owners.Where(o=>o.Id==ownerId).FirstOrDefault();
            var pokCatcreate = dataContext.Categories.Where(o=>o.Id==categoryId).FirstOrDefault();

            var PokemonOwner = new PokemonOwner{
                Owner = pokOwnCreate,
                Pokemon = pokemon
            };

            dataContext.Add(PokemonOwner);

            var PokemonCategory = new PokemonCategory{
                Category = pokCatcreate,
                Pokemon = pokemon
            };

            dataContext.Add(PokemonCategory);

            dataContext.Add(pokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            dataContext.Remove(pokemon);
            return Save();
        }

        public bool doesPokemonExist(int pokeid)
        {
            return dataContext.Pokemons.Any(p=>p.Id==pokeid);
        }

        public Pokemon GetPokemonById(int id)
        {
            return dataContext.Pokemons.Where(p=> p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string name)
        {
            return dataContext.Pokemons.Where(p=>p.Name==name).Include(p=>p.Reviews).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeid)
        {
           var reviews = dataContext.Reviews.Where(p=>p.Pokemon.Id==pokeid);
           if(reviews.Count()<=0)
           {
                return 0;
           }
           return ((decimal)reviews.Sum(p=>p.Rating)/reviews.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return dataContext.Pokemons.Include(p=>p.Reviews).OrderBy(p => p.Id).ToList();
        }

        public Pokemon GetPokemonTrimToUpper(PokemonDTO pokemonCreate)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            dataContext.Update(pokemon);
            return Save();
        }
    }
}