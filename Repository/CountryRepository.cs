using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext dataContext;

        public CountryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool CountryExist(int countryId)
        {
            return dataContext.Countries.Any(c=>c.Id==countryId);
        }

        public bool DeleteCountry(Country country)
        {
            dataContext.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return dataContext.Countries.OrderBy(c=>c.Id).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return dataContext.Countries.Where(c=>c.Id==countryId).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return dataContext.Owners.Where(o=>o.Id==ownerId).Select(c=>c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountry(int countryId)
        {
            return dataContext.Owners.Where(c=>c.Country.Id==countryId).ToList();
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            dataContext.Update(country);
            return Save();
        }
    }
}