using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            dataContext.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return dataContext.Reviewers.Where(r=>r.Id==reviewerId).Include(c=>c.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return dataContext.Reviewers.OrderBy(r=>r.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return dataContext.Reviews.Where(r=>r.Reviewer.Id==reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return dataContext.Reviewers.Any(r=>r.Id==reviewerId);
        }

        public bool Save()
        {
           var saved = dataContext.SaveChanges();
           return saved > 0 ? true : false;
        }
    }
}