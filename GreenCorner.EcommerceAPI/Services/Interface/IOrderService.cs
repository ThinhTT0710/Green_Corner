using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Services.Interface
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAll();
        Task<OrderDTO> GetById(int id);
        Task Add(OrderDTO item);
        Task Update(OrderDTO item);
		Task UpdateOrderStatus(int orderId, string newStatus);
		Task Delete(int orderId);
        Task<List<OrderDTO>> GetByUserID(string userID);
    }
}
