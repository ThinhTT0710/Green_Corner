using GreenCorner.BlogAPI.Data;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GreenCorner.BlogAPI.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly GreenCornerBlogContext _context;
        public BlogPostRepository(GreenCornerBlogContext context)
        {
            this._context = context;
        }

        public async Task AddBlog(BlogPost item)
        {
            item.CreatedAt = DateTime.Now;
            item.Status = "Pending";
            await _context.BlogPosts.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task BlogApproval(int id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null)
            {
                throw new KeyNotFoundException($"Blog with ID {id} not found.");
            }

            blog.Status = "Published";
            _context.BlogPosts.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task BlogReject(int id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null)
            {
                throw new KeyNotFoundException($"Blog with ID {id} not found.");
            }

            blog.Status = "Rejected";
            _context.BlogPosts.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlog(int id)
        {
            var product = await _context.BlogPosts.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Blog with ID {id} not found.");
            }

            _context.BlogPosts.Remove(product);
            await _context.SaveChangesAsync();

        }

        //ViewBlogPosts
        public async Task<IEnumerable<BlogPost>> GetAllBlogPost()
        {
            //return await _context.BlogPosts.ToListAsync();
            return await _context.BlogPosts
                         .Where(b => b.Status == "Published")
                         .ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetAllPendingPost()
        {
            return await _context.BlogPosts
                        .Where(b => b.Status == "Pending")
                        .ToListAsync();
        }

        public  async Task<IEnumerable<BlogPost>> GetBlogCreate(string userId)
        {
            return await _context.BlogPosts.Where(b => b.AuthorId == userId).ToListAsync();
        }

        public async Task<BlogPost> GetByBlogId(int id)
        {
            return await _context.BlogPosts
                .FirstOrDefaultAsync(p => p.BlogId == id)
                ?? throw new KeyNotFoundException($"Blog with ID {id} not found.");
        }

        public async Task UpdateBlog(BlogPost item)
        {
            var product = await _context.BlogPosts.FindAsync(item.BlogId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Blog with ID {item.BlogId} not found.");
            }else if(product.Status != "Pending")
            {
                throw new InvalidOperationException("Bài viết không thể chỉnh sửa.");
            }
            _context.Entry(product).CurrentValues.SetValues(item);
            product.CreatedAt = DateTime.Now;
            product.Status = "Pending";
            await _context.SaveChangesAsync();
        }
    }
}
