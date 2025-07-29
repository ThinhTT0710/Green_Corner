using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using System;
using System.Reflection.Metadata;
using static GreenCorner.MVC.Utility.SD;

namespace GreenCorner.MVC.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBaseService _baseService;
        public BlogPostService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AddBlog(BlogPostDTO blog)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = blog,
                Url = SD.BlogAPIBase + "/api/BlogPost"
            });
        }

        public async Task<ResponseDTO?> BlogApproval(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.BlogAPIBase + "/api/BlogPost/blogapproval?id=" + id
            });
        }

        public async Task<ResponseDTO?> BlogReject(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.BlogAPIBase + "/api/BlogPost/blogreject?id=" + id
            });
        }

        public async Task<ResponseDTO?> DeleteBlog(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.BlogAPIBase + "/api/BlogPost/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllBlogPost()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + "/api/BlogPost"
            });
        }

        public async Task<ResponseDTO?> GetAllPendingPost()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + "/api/BlogPost/pending"
            });
        }

        public async Task<ResponseDTO?> GetBlogCreate(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.BlogAPIBase}/api/BlogPost/blogcreate?userId={userId}"
            });
        }

        public async Task<ResponseDTO?> GetByBlogId(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + "/api/BlogPost/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateBlog(BlogPostDTO blog)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = blog,
                Url = SD.BlogAPIBase + "/api/BlogPost"
            });
        }
    }
}
