using AutoMapper;
using GreenCorner.ChatAPI.DTOs;
using GreenCorner.ChatAPI.Models;

namespace GreenCorner.ChatAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ChatMessageDTO, ChatMessage>().ReverseMap();
                config.CreateMap<NotificationDTO, Notification>().ReverseMap();

            });
            return mappingConfig;
        }
    }
}
