using AutoMapper;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration( config =>
            {
                config.CreateMap<ProductDTO, Product>().ReverseMap();
                config.CreateMap<CartDTO, Cart>().ReverseMap();
                config.CreateMap<OrderDTO, Order>().ReverseMap();
                config.CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
                config.CreateMap<WishListDTO, WishList>().ReverseMap();
            });
            
            return mappingConfig;
        }
    }
}
