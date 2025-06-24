using AutoMapper;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories;
using GreenCorner.BlogAPI.Services.Interface;
using GreenCorner.BlogAPI.Repositories.Interface;

namespace GreenCorner.BlogAPI.Services
{
    public class BlogFavoriteService : IBlogFavoriteService
    {
        private readonly IBlogFavoriteRepository _blogFavoriteRepository;
        private readonly IMapper _mapper;

        public BlogFavoriteService(IBlogFavoriteRepository blogFavoriteRepository, IMapper mapper)
        {
            _blogFavoriteRepository = blogFavoriteRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddFavoriteAsync(BlogFavoriteAddDTO dto)
        {
            var exists = await _blogFavoriteRepository.IsFavorited(dto.BlogId, dto.UserId);
            if (exists) return false;

            var entity = _mapper.Map<BlogFavorite>(dto);
            await _blogFavoriteRepository.AddFavorite(entity);
            return true;
        }

        public async Task<bool> RemoveFavoriteAsync(int blogId, string userId)
        {
            await _blogFavoriteRepository.RemoveFavorite(blogId, userId);
            return true;
        }

        public async Task<bool> IsFavoritedAsync(int blogId, string userId)
        {
            return await _blogFavoriteRepository.IsFavorited(blogId, userId);
        }

        public async Task<IEnumerable<BlogFavoriteDTO>> GetFavoritesByUserAsync(string userId)
        {
            var favorites = await _blogFavoriteRepository.GetFavoritesByUser(userId);
            return _mapper.Map<IEnumerable<BlogFavoriteDTO>>(favorites);
        }
    }
}
