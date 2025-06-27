using GreenCorner.EcommerceAPI.Data;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EcommerceAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly GreenCornerEcommerceContext _context;
        public OrderRepository(GreenCornerEcommerceContext context)
        {
            _context = context;
        }

        public async Task Add(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new Exception("Không tìm thấy đơn hàng");
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetByUserId(string userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task Update(Order item)
        {
            var order = await _context.FindAsync<Order>(item.OrderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            _context.Entry(order).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
        }

		public async Task UpdateOrderStatus(int orderId, string newStatus)
		{
			var order = await _context.Orders.FindAsync(orderId);
			if (order == null)
			{
				throw new Exception("Order not found");
			}

			order.Status = newStatus;
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
		}
	}
}
