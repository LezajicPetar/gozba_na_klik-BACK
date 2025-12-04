using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly GozbaDbContext _dbContext;

        public ReviewRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();

            return review;
        }

        public Task<Review?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Review>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _dbContext.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r =>  r.Id == id);
        }

        public Task<Review?> UpdateAsync(Review entity)
        {
            throw new NotImplementedException();
        }
    }
}
