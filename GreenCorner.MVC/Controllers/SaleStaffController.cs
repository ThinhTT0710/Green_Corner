using ClosedXML.Excel;
using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
	public class SaleStaffController : Controller
	{
		private IOrderService _orderService;
        private readonly IProductService _productService;
        public SaleStaffController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public IActionResult Index()
		{
			return View();
		}
        public async Task<IActionResult> ProductList()
        {
            List<ProductDTO> listProduct = new();
            ResponseDTO? response = await _productService.GetAllProduct();
            if (response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listProduct);
        }

        public async Task<IActionResult> ViewAllProduct()
        {
            List<ProductDTO> listProduct = new();
            ResponseDTO? response = await _productService.GetAllProduct();
            if (response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listProduct);
        }

        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDTO product = new();
            var response = await _productService.GetByProductId(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                return View(product);
            }

            TempData["error"] = response?.Message ?? "Không tìm thấy sản phẩm.";
            return RedirectToAction("ViewAllProduct");
        }


        public async Task<IActionResult> OrderList() 
		{
			ResponseDTO? response = await _orderService.GetAllOrder();
			try
			{
				if (response != null && response.IsSuccess)
				{
					List<OrderDTO> orderList = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Result.ToString());
					return View(orderList);
				}
				else 
				{
					TempData["error"] = response.Message;
					return RedirectToAction("Index", "SaleStaff");
				}
			}
			catch (Exception ex) 
			{
				TempData["error"] = response.Message;
				return RedirectToAction("Index", "SaleStaff");
			}
		}

		[HttpGet]
		public async Task<IActionResult> OrderDetail(int id)
		{
			ResponseDTO? response = await _orderService.GetOrderByID(id);
			try
			{
				if (response != null && response.IsSuccess)
				{
					OrderDTO order = JsonConvert.DeserializeObject<OrderDTO>(response.Result.ToString());
					return View(order);
				}
				else
				{
					TempData["error"] = response.Message;
					return RedirectToAction("Index", "SaleStaff");
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = response.Message;
				return RedirectToAction("Index", "SaleStaff");
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateOrderStatus(OrderDTO order)
		{
			try
			{
				var response = await _orderService.UpdateOrderStatus(order.OrderId, order.Status);

                if (response != null && response.IsSuccess)
				{
					TempData["success"] = response.Message;
					return RedirectToAction("OrderList", "SaleStaff");
				}
				else
				{
					TempData["UpdateErrorMessage"] = response?.Message ?? "Cập nhật order status thất bại";
				}
			}
			catch (Exception ex)
			{
				TempData["UpdateErrorMessage"] = ex.Message;
			}
			return RedirectToAction("OrderDetail", new { id = order.OrderId });
		}

        public async Task<IActionResult> ExportToExcel()
        {
            ResponseDTO? response = await _productService.GetAllProduct();
            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = "Không thể lấy danh sách sản phẩm.";
                return RedirectToAction(nameof(ViewAllProduct));
            }

            var products = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachSanPham");

                // Header
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 2).Value = "Tên sản phẩm";
                worksheet.Cell(1, 3).Value = "Giá gốc";
                worksheet.Cell(1, 4).Value = "Giảm giá (%)";
                worksheet.Cell(1, 5).Value = "Giá sau giảm";
                worksheet.Cell(1, 6).Value = "Số lượng";
                worksheet.Cell(1, 7).Value = "Danh mục";
                worksheet.Cell(1, 8).Value = "Thương hiệu";
                worksheet.Cell(1, 9).Value = "Xuất xứ";
                worksheet.Cell(1, 10).Value = "Ngày tạo";

                int row = 2;
                for (int i = 0; i < products.Count; i++)
                {
                    var p = products[i];
                    var discountedPrice = p.Discount.HasValue ? p.Price * (1 - p.Discount.Value / 100m) : p.Price;

                    worksheet.Cell(row, 1).Value = i + 1;
                    worksheet.Cell(row, 2).Value = p.Name;
                    worksheet.Cell(row, 3).Value = p.Price;
                    worksheet.Cell(row, 4).Value = p.Discount ?? 0;
                    worksheet.Cell(row, 5).Value = discountedPrice;
                    worksheet.Cell(row, 6).Value = p.Quantity;
                    worksheet.Cell(row, 7).Value = p.Category ?? "Không rõ";
                    worksheet.Cell(row, 8).Value = p.Brand ?? "Không rõ";
                    worksheet.Cell(row, 9).Value = p.Origin ?? "Không rõ";
                    worksheet.Cell(row, 10).Value = p.CreatedAt?.ToString("dd/MM/yyyy") ?? "Không rõ";

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "DanhSachSanPham.xlsx");
                }
            }
        }
    }
}
