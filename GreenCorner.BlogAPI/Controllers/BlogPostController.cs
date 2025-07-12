using GreenCorner.BlogAPI.Models.DTO;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace GreenCorner.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly ResponseDTO _responseDTO;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> ViewBlogPosts()
        {
            try
            {
                var blogPosts = await _blogPostService.GetAllBlogPost();
                _responseDTO.Result = blogPosts;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("pending")]
        public async Task<ResponseDTO> ViewPendingPosts()
        {
            try
            {
                var blogPosts = await _blogPostService.GetAllPendingPost();
                _responseDTO.Result = blogPosts;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO> ViewBlogDetails(int id)
        {
            try
            {
                var blog = await _blogPostService.GetByBlogId(id);
                _responseDTO.Result = blog;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Result = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost]
        public async Task<ResponseDTO> CreateBlog([FromBody] BlogPostDTO product)
        {
            try
            {
                await _blogPostService.AddBlog(product);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpPut]
        public async Task<ResponseDTO> UpdateBlog([FromBody] BlogPostDTO productDto)
        {
            try
            {
                await _blogPostService.UpdateBlog(productDto);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteBlog(int id)
        {
            try
            {
                await _blogPostService.DeleteBlog(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost("blogapproval")]
        public async Task<ResponseDTO> BlogApproval(int id)
        {
            try
            {
                await _blogPostService.BlogApproval(id);
                return _responseDTO;
            }
            catch(Exception ex) 
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("blogcreate")]
        public async Task<ResponseDTO> ViewBlogCreate(string userId)
        {
            try
            {
                var blogPosts = await _blogPostService.GetBlogCreate(userId);
                _responseDTO.Result = blogPosts;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
