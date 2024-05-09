using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext dataContext;

        public OwnerRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool CreateOwner(int pokeId, Owner owner)
        {
            var ownPokeCreate = dataContext.Pokemons.Where(p=>p.Id==pokeId).FirstOrDefault();
            
            var PokeOwn = new PokemonOwner{
                Owner = owner,
                Pokemon = ownPokeCreate
            };

            dataContext.Add(PokeOwn);
            dataContext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            dataContext.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return dataContext.Owners.Where(o=>o.Id==ownerId).Include(c=>c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return dataContext.Owners.OrderBy(o=>o.Id).Include(c=>c.Country).ToList();
        }

        public ICollection<Owner> GetOwnersOfaPokemon(int pokeId)
        {
            return dataContext.PokemonOwners.Where(p=>p.PokemonId == pokeId).Select(o=>o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsOfaOwner(int ownerId)
        {
            return dataContext.PokemonOwners.Where(o=>o.OwnerId==ownerId).Select(p=>p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return dataContext.Owners.Any(o=>o.Id==ownerId);
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            dataContext.Update(owner);
            return Save();
        }
    }
}