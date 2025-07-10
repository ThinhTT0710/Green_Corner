using GreenCorner.EcommerceAPI.Data;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;
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

        //dashboard
        public async Task<int> TotalOrdersComplete()
        {
            return await _context.Orders.Where(x => x.Status == "Hoàn thành").CountAsync();
        }

        public async Task<int> TotalOrdersWaiting()
        {
            return await _context.Orders.Where(x => x.Status == "Chờ xác nhận").CountAsync();
        }

        public async Task<int> TotalSales()
        {
            var total = (int) await _context.Orders
            .Where(o => o.Status == "Hoàn thành")
            .SumAsync(o => o.TotalMoney);
            return total;
        }

        public async Task<int> GetTotalMoneyByMonth()
        {
            var now = DateTime.UtcNow;
            var totalByMonth = (int)await _context.Orders
                .Where(o => o.CreatedAt.Value.Month == now.Month &&
                            o.CreatedAt.Value.Year == now.Year &&
                            o.Status == "Hoàn thành")
                .SumAsync(o => o.TotalMoney);
            return totalByMonth;
        }

        public async Task<MonthlyAnalyticsDto> GetMonthlySalesAnalytics(int year)
        {
            var productsSoldByMonth = new int[12];
            var completedOrdersByMonth = new int[12];
            var otherStatusOrdersByMonth = new int[12];

            var ordersInYear = await _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Year == year)
                .ToListAsync();

            var groupedByMonth = ordersInYear.GroupBy(o => o.CreatedAt.Value.Month);

            foreach (var group in groupedByMonth)
            {
                int monthIndex = group.Key - 1; 

                var completedOrders = group.Where(o => o.Status == "Hoàn thành").ToList();
                completedOrdersByMonth[monthIndex] = completedOrders.Count;

                otherStatusOrdersByMonth[monthIndex] = group.Count() - completedOrders.Count;

                int totalProductsSold = completedOrders
                    .SelectMany(o => o.OrderDetails)
                    .Sum(od => od.Quantity);

                productsSoldByMonth[monthIndex] = totalProductsSold;
            }

            var analyticsData = new MonthlyAnalyticsDto
            {
                ProductsSold = productsSoldByMonth.ToList(),
                CompletedOrders = completedOrdersByMonth.ToList(),
                OtherStatusOrders = otherStatusOrdersByMonth.ToList()
            };

            return analyticsData;
        }
        public async Task<CategorySalesDto> GetSalesByCategory()
        {
            var salesByCategory = await _context.OrderDetails
                .Join(_context.Orders,
                      orderDetail => orderDetail.OrderId,
                      order => order.OrderId,
                      (orderDetail, order) => new { orderDetail, order })
                .Join(_context.Products,
                      joined => joined.orderDetail.ProductId,
                      product => product.ProductId,
                      (joined, product) => new { joined.orderDetail, joined.order, product })
                .Where(x => x.order.Status == "Hoàn thành" && x.product.IsDeleted == false)
                .GroupBy(x => x.product.Category)
                .Select(group => new
                {
                    CategoryName = group.Key,
                    TotalQuantity = group.Sum(x => x.orderDetail.Quantity)
                })
                .OrderByDescending(result => result.TotalQuantity)
                .ToListAsync();

            var dto = new CategorySalesDto
            {
                CategoryNames = salesByCategory.Select(s => s.CategoryName).ToList(),
                QuantitiesSold = salesByCategory.Select(s => s.TotalQuantity).ToList()
            };

            return dto;
        }
    }
}
