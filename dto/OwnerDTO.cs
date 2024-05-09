using test.Models;

namespace test.dto
{
    public class OwnerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public CountryDTO Country {get; set;}
    }
}