using GreenCorner.EcommerceAPI.Data;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EcommerceAPI.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly GreenCornerEcommerceContext _context;

        public OrderDetailRepository(GreenCornerEcommerceContext context)
        {
            _context = context;
        }

        public async Task Add(OrderDetail orderDetail)
        {
             _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var orderdetail = await _context.OrderDetails.FindAsync(id);
            if (orderdetail == null)
            {
                throw new Exception("OrderDetail not found");
            }
            _context.OrderDetails.Remove(orderdetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAll()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<OrderDetail> GetById(int id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderID(int OrderID)
        {
            return await _context.OrderDetails.Where(x => x.OrderId == OrderID).ToListAsync();
        }

        public async Task Update(OrderDetail orderDetail)
        {
            var orderdetail = await _context.OrderDetails.FindAsync(orderDetail.OrderDetailId);
            if (orderdetail == null)
            {
                throw new Exception("OrderDetail not found");
            }
            _context.Entry(orderdetail).CurrentValues.SetValues(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
