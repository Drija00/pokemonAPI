using Microsoft.AspNetCore.Mvc.ModelBinding;
using test.Models;

namespace test.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnersOfaPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonsOfaOwner(int ownerId);
        bool OwnerExists(int ownerId);
        public bool CreateOwner(int pokeId, Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        public bool Save();
    }
}