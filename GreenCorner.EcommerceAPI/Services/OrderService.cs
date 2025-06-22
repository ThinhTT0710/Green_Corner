using AutoMapper;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using GreenCorner.EcommerceAPI.Services.Interface;

namespace GreenCorner.EcommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProductService productService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task Add(OrderDTO item)
        {
            Order order = _mapper.Map<Order>(item);
            await _orderRepository.Add(order);
            foreach (var orderDetailDTO in item.OrderDetailsDTO)
            {
                OrderDetail orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
                orderDetail.OrderId = order.OrderId;
                await _orderDetailRepository.Add(orderDetail);
            }
        }

		public async Task Delete(int orderId)
		{
			Order order = await _orderRepository.GetById(orderId);
			if (order == null)
			{
				throw new Exception($"Order with ID {orderId} not found.");
			}
			if (!order.Status.Equals("Chờ xác nhận"))
			{
				throw new Exception("Đơn hàng đã được xác nhận và không thể xóa.");
			}
			var orderDetails = await _orderDetailRepository.GetByOrderID(orderId);

			foreach (var orderDetail in orderDetails)
			{
				await _orderDetailRepository.Delete(orderDetail.OrderDetailId);
			}

			await _orderRepository.Delete(orderId);
		}

		public async Task<List<OrderDTO>> GetAll()
        {
            var orders = await _orderRepository.GetAll();
            List<OrderDTO> orderDTOs = _mapper.Map<List<OrderDTO>>(orders);
            foreach (var orderDTO in orderDTOs)
            {
                var orderDetails = await _orderDetailRepository.GetByOrderID(orderDTO.OrderId);
                var orderDetailDTOs = _mapper.Map<List<OrderDetailDTO>>(orderDetails);
                orderDTO.OrderDetailsDTO = orderDetailDTOs;
                foreach (var orderDetail in orderDTO.OrderDetailsDTO)
                {
                    var product = await _productService.GetByProductId(orderDetail.ProductId);
                    orderDetail.ProductDTO = product;
                }
            }
            return orderDTOs;
        }

        public async Task<OrderDTO> GetById(int id)
        {
            var order = await _orderRepository.GetById(id);
            var orderDetails = await _orderDetailRepository.GetByOrderID(order.OrderId);
            order.OrderDetails = orderDetails.ToList();
            var orderDTO = _mapper.Map<OrderDTO>(order);
            var orderDetailDTOs = _mapper.Map<List<OrderDetailDTO>>(orderDetails);
            orderDTO.OrderDetailsDTO = orderDetailDTOs;
            foreach (var orderDetail in orderDTO.OrderDetailsDTO)
            {
                var product = await _productService.GetByProductId(orderDetail.ProductId);
                orderDetail.ProductDTO = product;
            }
            return orderDTO;
        }

        public async Task<List<OrderDTO>> GetByUserID(string userID)
        {
            var orderDTOs = await GetAll();
            List<OrderDTO> orderDTOsByUserID = new List<OrderDTO>();
            List<OrderDetailDTO> listOrderDetailDTOs = new List<OrderDetailDTO>();
            foreach (var item in orderDTOs)
            {
                if (item.UserId == userID)
                {
                    var orderDetails = await _orderDetailRepository.GetByOrderID(item.OrderId);
                    var orderDetailDTOs = _mapper.Map<List<OrderDetailDTO>>(orderDetails);
                    item.OrderDetailsDTO = orderDetailDTOs;
                    foreach (var orderDetail in item.OrderDetailsDTO)
                    {
                        var product = await _productService.GetByProductId(orderDetail.ProductId);
                        orderDetail.ProductDTO = product;
                    }
                    orderDTOsByUserID.Add(item);
                }
            }
            return orderDTOsByUserID;
        }

        public async Task Update(OrderDTO item)
        {
            var order = _mapper.Map<Order>(item);
            await _orderRepository.Update(order);
            var orderDetails = _mapper.Map<List<OrderDetail>>(item.OrderDetailsDTO);
            if (orderDetails.Count > 0)
            {
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderDetailId != 0)
                    {
                        await _orderDetailRepository.Update(orderDetail);
                    }
                }
            }
        }

		public async Task UpdateOrderStatus(int orderId, string newStatus)
		{
			await _orderRepository.UpdateOrderStatus(orderId, newStatus);
		}
	}
}
