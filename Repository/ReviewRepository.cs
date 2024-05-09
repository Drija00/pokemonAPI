using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool CreateReview(Review review)
        {
            dataContext.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            dataContext.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            dataContext.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return dataContext.Reviews.Where(r=>r.Id==reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return dataContext.Reviews.OrderBy(r=>r.Id).ToList();
        }

        public ICollection<Review> GetReviewsOfPokemon(int pokeId)
        {
            return dataContext.Reviews.Where(r=>r.Pokemon.Id==pokeId).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return dataContext.Reviews.Any(r=>r.Id==reviewId);
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}