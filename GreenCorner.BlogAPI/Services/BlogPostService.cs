using AutoMapper;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Repositories.Interface;
using GreenCorner.BlogAPI.Services.Interface;
using System.Runtime.CompilerServices;

namespace GreenCorner.BlogAPI.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IMapper _mapper;

        public BlogPostService(IBlogPostRepository blogPostRepository, IMapper mapper)
        {
            _blogPostRepository = blogPostRepository;
            _mapper = mapper;
        }

        public async Task AddBlog(BlogPostDTO blog)
        {
            BlogPost blogPost = _mapper.Map<BlogPost>(blog);
            await _blogPostRepository.AddBlog(blogPost);
        }

        public async Task BlogApproval(int id)
        {
            await _blogPostRepository.BlogApproval(id);
        }

        public async Task BlogReject(int id)
        {
            await _blogPostRepository.BlogReject(id);
        }

        public async Task DeleteBlog(int id)
        {
            await _blogPostRepository.DeleteBlog(id);
        }

        public async Task<IEnumerable<BlogPostDTO>> GetAllBlogPost()
        {
            var blogPosts = await _blogPostRepository.GetAllBlogPost();
            return _mapper.Map<List<BlogPostDTO>>(blogPosts);
        }

        public async Task<IEnumerable<BlogPostDTO>> GetAllPendingPost()
        {
            var blogPosts = await _blogPostRepository.GetAllPendingPost();
            return _mapper.Map<List<BlogPostDTO>>(blogPosts);
        }

        public  async Task<IEnumerable<BlogPostDTO>> GetBlogCreate(string userId)
        {
            var blogPosts = await _blogPostRepository.GetBlogCreate(userId);
            return _mapper.Map<List<BlogPostDTO>>(blogPosts);
        }

        public async Task<BlogPostDTO> GetByBlogId(int id)
        {
            var blog = await _blogPostRepository.GetByBlogId(id);
            return _mapper.Map<BlogPostDTO>(blog);
        }

        public async Task UpdateBlog(BlogPostDTO blog)
        {
            BlogPost product = _mapper.Map<BlogPost>(blog);
            await _blogPostRepository.UpdateBlog(product);
        }
    }
}
