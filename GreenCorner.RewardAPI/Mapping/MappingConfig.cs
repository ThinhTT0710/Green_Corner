using AutoMapper;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<VoucherDTO, Voucher>().ReverseMap();
                config.CreateMap<RewardPointDTO, RewardPoint>().ReverseMap();
                config.CreateMap<PointTransactionDTO, PointTransaction>().ReverseMap();
            });

            return mappingConfig;
        }
    }

}

