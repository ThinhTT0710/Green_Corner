using AutoMapper;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration EventMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<EventDTO, CleanupEvent>().ReverseMap();
                config.CreateMap<EventReviewDTO, EventReview>().ReverseMap();
                config.CreateMap<LeaderReviewDTO, LeaderReview>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
