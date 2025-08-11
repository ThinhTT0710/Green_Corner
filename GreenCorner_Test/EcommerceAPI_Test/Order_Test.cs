using GreenCorner.EcommerceAPI.Controllers;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.EcommerceAPI_Test
{
    public class Order_Test
    {
        private readonly Mock<IOrderService> _mockService = new();
        private readonly OrderController _controller;

        public Order_Test()
        {
            _controller = new OrderController(_mockService.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsList()
        {
            var orders = new List<OrderDTO> { new OrderDTO { OrderId = 1 } };
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(orders);

            var result = await _controller.GetOrders();

            Assert.True(result.IsSuccess);
            Assert.Equal(orders, result.Result);
        }

        [Fact]
        public async Task GetOrderById_ReturnsCorrectOrder()
        {
            var order = new OrderDTO { OrderId = 2 };
            _mockService.Setup(s => s.GetById(2)).ReturnsAsync(order);

            var result = await _controller.GetOrderById(2);

            Assert.True(result.IsSuccess);
            Assert.Equal(order, result.Result);
        }

        [Fact]
        public async Task CreateOrder_ValidInput_ReturnsSuccess()
        {
            var dto = new OrderDTO { OrderId = 3 };
            _mockService.Setup(s => s.Add(dto)).Returns(Task.CompletedTask);

            var result = await _controller.CreateOrder(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateOrder_ValidInput_ReturnsSuccess()
        {
            var dto = new OrderDTO { OrderId = 4 };
            _mockService.Setup(s => s.Update(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateOrder(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateOrderStatus_ValidRequest_ReturnsSuccessMessage()
        {
            var statusDto = new UpdateOrderStatusDTO { OrderId = 5, OrderStatus = "Shipped" };
            _mockService.Setup(s => s.UpdateOrderStatus(5, "Shipped")).Returns(Task.CompletedTask);

            var result = await _controller.UpdateOrderStatus(statusDto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Cập nhập thành công trạng thái của đơn hàng", result.Message);
        }

        [Fact]
        public async Task DeleteOrder_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.Delete(6)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteOrder(6);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetOrderByUserId_ValidUser_ReturnsOrders()
        {
            var orders = new List<OrderDTO> { new OrderDTO { UserId = "u1" } };
            _mockService.Setup(s => s.GetByUserID("u1")).ReturnsAsync(orders);

            var result = await _controller.GetOrderByUserId("u1");

            Assert.True(result.IsSuccess);
            Assert.Equal(orders, result.Result);
        }

        [Fact]
        public async Task TotalOrdersComplete_ReturnsInt()
        {
            _mockService.Setup(s => s.TotalOrdersComplete()).ReturnsAsync(88);

            var result = await _controller.TotalOrdersComplete();

            Assert.True(result.IsSuccess);
            Assert.Equal(88, result.Result);
        }

        [Fact]
        public async Task TotalOrdersWaiting_ReturnsInt()
        {
            _mockService.Setup(s => s.TotalOrdersWaiting()).ReturnsAsync(22);

            var result = await _controller.TotalOrdersWaiting();

            Assert.True(result.IsSuccess);
            Assert.Equal(22, result.Result);
        }

        [Fact]
        public async Task GetTrendingProduct_ReturnsTopProducts()
        {
            var topProducts = Enumerable.Range(1, 10).Select(i => new ProductDTO { ProductId = i }).ToList();
            _mockService.Setup(s => s.Top10BestSelling()).ReturnsAsync(topProducts);

            var result = await _controller.GetTrendingProduct();

            Assert.True(result.IsSuccess);
            Assert.Equal(topProducts.Count, ((List<ProductDTO>)result.Result).Count);
        }
        [Fact]
        public async Task CreateOrder_ValidInput_ReturnsSuccessully()
        {
            var dto = new OrderDTO { OrderId = 3 };
            _mockService.Setup(s => s.Add(dto)).Returns(Task.CompletedTask);

            var result = await _controller.CreateOrder(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateOrder_ValidInput_ReturnsSuccessfully()
        {
            var dto = new OrderDTO { OrderId = 4 };
            _mockService.Setup(s => s.Update(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateOrder(dto);

            Assert.True(result.IsSuccess);
        }

    }
}
