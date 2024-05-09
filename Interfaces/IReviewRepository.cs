using test.dto;
using test.Models;

namespace test.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfPokemon(int pokeId);
        bool ReviewExists(int reviewId);
        bool CreateReview(Review review);
        bool Save();
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
    }
}