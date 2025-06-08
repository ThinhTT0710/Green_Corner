using AutoMapper;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Models.DTOs;

namespace GreenCorner.BlogAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration( config =>
            {
                config.CreateMap<BlogPostDTO, BlogPost>().ReverseMap();
                config.CreateMap<BlogFavoriteDTO, BlogFavorite>().ReverseMap();
                config.CreateMap<BlogFavoriteAddDTO, BlogFavorite>().ReverseMap();
                config.CreateMap<BlogReportDTO, BlogReport>().ReverseMap();
                config.CreateMap<FeedbackDTO, Feedback>().ReverseMap();
                config.CreateMap<ReportDTO, Report>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
