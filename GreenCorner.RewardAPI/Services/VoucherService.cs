using AutoMapper;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories.Interface;
using GreenCorner.RewardAPI.Services.Interface;

namespace GreenCorner.RewardAPI.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public VoucherService(IVoucherRepository voucherRepository, IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _mapper = mapper;
        }

        
       
        public async Task CreateVoucher(VoucherDTO voucherDTO)
        {
            Voucher voucher = _mapper.Map<Voucher>(voucherDTO);
            await _voucherRepository.CreateVoucher(voucher);
        }

        public async Task DeleteVoucher(int id)
        {
            await _voucherRepository.DeleteVoucher(id);
        }
        

        public async Task<VoucherDTO> GetRewardDetail(int id)
        {
            var voucher = await _voucherRepository.GetRewardDetail(id);
            return _mapper.Map<VoucherDTO>(voucher);

        }

        public async Task UpdateVoucher(VoucherDTO voucherDTO)
        {
            Voucher voucher = _mapper.Map<Voucher>(voucherDTO);
            await _voucherRepository.UpdateVoucher(voucher);
        }

        public async Task<IEnumerable<VoucherDTO>> GetAllRewards()
        {

            var vouchers = await _voucherRepository.GetAllRewards();
            return _mapper.Map<List<VoucherDTO>>(vouchers);
        }

        public async Task<IEnumerable<VoucherDTO>> GetAllVouchers()
        {

            var vouchers = await _voucherRepository.GetAllVouchers();
            return _mapper.Map<List<VoucherDTO>>(vouchers);
        }
    }

}

