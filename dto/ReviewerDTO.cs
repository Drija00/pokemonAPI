using test.Models;

namespace test.dto
{
    public class ReviewerDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}