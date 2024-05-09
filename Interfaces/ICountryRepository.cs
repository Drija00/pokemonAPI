using test.Models;

namespace test.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersByCountry(int countryId);
        bool CountryExist(int countryId);
        bool DeleteCountry(Country country);
        bool UpdateCountry(Country country);
        bool Save();
    }
}