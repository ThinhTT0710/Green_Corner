using AutoMapper;
using GreenCorner.EventAPI.DTOs;
using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TrashEventDTO, TrashEvent>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
