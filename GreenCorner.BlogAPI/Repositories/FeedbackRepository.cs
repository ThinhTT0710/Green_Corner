using GreenCorner.BlogAPI.Data;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.BlogAPI.Repositories
{
    public class FeedbackRepository:IFeedbackRepository
    {
        private readonly GreenCornerBlogContext _context;
        public FeedbackRepository(GreenCornerBlogContext context)
        {
            _context = context;
        }
        public async Task SubmitFeedback(Feedback feedback)
        {
            feedback.CreatedAt = DateTime.Now;
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedback()
        {
            return await _context.Feedbacks.ToListAsync();
        }
    }
}
