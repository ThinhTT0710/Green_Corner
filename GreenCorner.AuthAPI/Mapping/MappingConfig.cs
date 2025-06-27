using AutoMapper;
using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration( config =>
            {
                config.CreateMap<SystemLogDTO, SystemLog>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
