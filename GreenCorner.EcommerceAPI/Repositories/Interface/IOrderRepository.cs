using GreenCorner.EcommerceAPI.Models;

namespace GreenCorner.EcommerceAPI.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(int id);
        Task Add(Order order);
        Task Update(Order order);
		Task UpdateOrderStatus(int orderId, string newStatus);
		Task Delete(int id);
        Task<IEnumerable<Order>> GetByUserId(string userId);
	}
}
