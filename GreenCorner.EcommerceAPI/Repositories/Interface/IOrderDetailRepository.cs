using GreenCorner.EcommerceAPI.Models;

namespace GreenCorner.EcommerceAPI.Repositories.Interface
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAll();
        Task<OrderDetail> GetById(int id);
        Task Add(OrderDetail orderDetail);
        Task Update(OrderDetail orderDetail);
        Task Delete(int id);
        Task<IEnumerable<OrderDetail>> GetByOrderID(int OrderDetailId);
        Task<bool> HasRestrictedOrdersByProductId(int productId);
    }
}
