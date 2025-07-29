using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Admin;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace GreenCorner.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productservice;
        private readonly IRewardPointService _rewardPointService;
        private readonly IPointTransactionService _pointTransactionService;
        private readonly IVolunteerService _volunteerService;
        private readonly IEventService _eventService;
        private readonly ITrashEventService _trashEventService;
        public AdminController(IUserService userService, IAdminService adminService, IOrderService orderService, IProductService productservice, IRewardPointService rewardPointService, IPointTransactionService pointTransactionService, IVolunteerService volunteerService, IEventService eventService, ITrashEventService trashEventService)
        {
            _userService = userService;
            _adminService = adminService;
            _orderService = orderService;
            _productservice = productservice;
            _rewardPointService = rewardPointService;
            _pointTransactionService = pointTransactionService;
            _volunteerService = volunteerService;
            _eventService = eventService;
            _trashEventService = trashEventService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole(SD.RoleCustomer))
            {
                    return RedirectToAction("AccessDenied", "Auth");
            }
            int totalOrderComplete = 0;
            int totalOrderWaiting = 0;
            int totalSales = 0;
            int totalMoneyInMonth = 0;
            int TotalEvent = 0;
            int TotalOpenEvent = 0;
            int TotalVolunteer = 0;
            int TotalPedingVolunteer = 0;

            MonthlyAnalyticsDto chartData = new();
            CategorySalesDto categorySalesData = new CategorySalesDto();
            List<BestSellingProductDTO> bestSelling = new();
            List<ProductDTO> outOfStockProducts = new();
            List<EventDTO> EventOpenList = new List<EventDTO>();
            List<TrashEventDTO> TrashEventList = new List<TrashEventDTO>();
            EventMonthlyAnalyticsDto ChartData2 = new EventMonthlyAnalyticsDto();


            ResponseDTO? responseOrderComplete = await _orderService.TotalOrdersComplete();
            if (responseOrderComplete != null && responseOrderComplete.IsSuccess)
            {
                totalOrderComplete = Convert.ToInt32(responseOrderComplete.Result);
            }
            else
            {
                TempData["error"] = responseOrderComplete.Message == null ? "Error" : responseOrderComplete.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseOrderWaiting = await _orderService.TotalOrdersWaiting();
            if (responseOrderWaiting != null && responseOrderWaiting.IsSuccess)
            {
                totalOrderWaiting = Convert.ToInt32(responseOrderWaiting.Result);
            }
            else
            {
                TempData["error"] = responseOrderWaiting.Message == null ? "Error" : responseOrderWaiting.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseTotalSales = await _orderService.TotalSales();
            if (responseTotalSales != null && responseTotalSales.IsSuccess)
            {
                totalSales = Convert.ToInt32(responseTotalSales.Result);
            }
            else
            {
                TempData["error"] = responseTotalSales.Message == null ? "Error" : responseTotalSales.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseTotalMoney = await _orderService.GetTotalMoneyByMonth();
            if (responseTotalMoney != null && responseTotalMoney.IsSuccess)
            {
                totalMoneyInMonth = Convert.ToInt32(responseTotalMoney.Result);
            }
            else
            {
                TempData["error"] = responseTotalMoney.Message == null ? "Error" : responseTotalMoney.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseChart = await _adminService.GetMonthlyAnalytics();
            if (responseChart != null && responseChart.IsSuccess)
            {
                chartData = JsonConvert.DeserializeObject<MonthlyAnalyticsDto>(Convert.ToString(responseChart.Result));
            }
            else
            {
                TempData["error"] = responseChart?.Message ?? "Error fetching chart data";
            }
            ResponseDTO? responseCategorySales = await _adminService.GetSalesByCategory();
            if (responseCategorySales != null && responseCategorySales.IsSuccess)
            {
                categorySalesData = JsonConvert.DeserializeObject<CategorySalesDto>(Convert.ToString(responseCategorySales.Result));
            }
            else
            {
                TempData["error"] = responseCategorySales?.Message ?? "Error fetching category sales data";
            }
            ResponseDTO? responseBestSeller = await _orderService.GetBestSellingProduct();
            if (responseBestSeller != null && responseBestSeller.IsSuccess)
            {
                bestSelling = JsonConvert.DeserializeObject<List<BestSellingProductDTO>>(responseBestSeller.Result.ToString());
            }
            else
            {
                TempData["error"] = responseBestSeller.Message == null ? "Error" : responseBestSeller.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseOutOfStock = await _productservice.OutOfStockProduct();
            if (responseOutOfStock != null && responseOutOfStock.IsSuccess)
            {
                outOfStockProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(responseOutOfStock.Result.ToString());
            }
            else
            {
                TempData["error"] = responseBestSeller.Message == null ? "Error" : responseOutOfStock.Message;
                return RedirectToAction("Index", "Admin");
            }

            var responseTotalEvents = await _eventService.GetAllEvent();

            if (responseTotalEvents != null && responseTotalEvents.IsSuccess)
            {
                var eventListJson = responseTotalEvents.Result?.ToString();

                if (!string.IsNullOrEmpty(eventListJson))
                {
                    var eventList = JsonConvert.DeserializeObject<List<EventDTO>>(eventListJson);
                    if (eventList != null)
                    {
                        TotalEvent = eventList.Count;
                        TotalOpenEvent = eventList.Count(e => e.Status == "Closed");
                    }
                }
            }

            var responseTotalVolunteers = await _volunteerService.GetAllVolunteer();

            if (responseTotalVolunteers != null && responseTotalVolunteers.IsSuccess)
            {
                var volunteerListJson = responseTotalVolunteers.Result?.ToString();

                if (!string.IsNullOrEmpty(volunteerListJson))
                {
                    var volunteerList = JsonConvert.DeserializeObject<List<EventDTO>>(volunteerListJson);
                    if (volunteerList != null)
                    {
                        TotalVolunteer = volunteerList.Count;
                        TotalPedingVolunteer = volunteerList.Count(v => v.Status == "Pending");
                    }
                }
            }

            ResponseDTO? responseEventOpen = await _eventService.GetOpenEvent();
            if (responseEventOpen != null && responseEventOpen.IsSuccess)
            {
                var eventOpenList = JsonConvert.DeserializeObject<List<EventDTO>>(responseEventOpen.Result.ToString());
                EventOpenList = eventOpenList;
            }
            else
            {
                TempData["error"] = responseEventOpen.Message == null ? "Error" : responseEventOpen.Message;
                return RedirectToAction("Index", "Admin");
            }

            ResponseDTO? response = await _trashEventService.GetAllTrashEvent();
            if (response != null && response.IsSuccess)
            {
                var trashEvents = JsonConvert.DeserializeObject<List<TrashEventDTO>>(response.Result.ToString());
                var pendingTrashEvents = trashEvents
                .Where(e => e.Status != null && e.Status.Equals("Chờ xác nhận", StringComparison.OrdinalIgnoreCase))
                .ToList();
                TrashEventList = pendingTrashEvents;
            }

            ResponseDTO? responseChart2 = await _adminService.EventMonthlyAnalytics();
            if (responseChart2 != null && responseChart2.IsSuccess)
            {
                var chartData2 = JsonConvert.DeserializeObject<EventMonthlyAnalyticsDto>(Convert.ToString(responseChart2.Result));
                ChartData2 = chartData2;
            }
            else
            {
                TempData["error"] = responseChart?.Message ?? "Error fetching chart data";
            }

            var viewModel = new AdminViewModel
            {
                TotalOrdersComplete = totalOrderComplete,
                TotalOrdersWaiting = totalOrderWaiting,
                TotalSales = totalSales,
                TotalMoneyByMonth = totalMoneyInMonth,
                BestSellingProducts = bestSelling,
                OutOfStockProduct = outOfStockProducts,
                ChartData = chartData,
                CategorySalesChartData = categorySalesData,
                TotalEvent = TotalEvent,
                TotalOpenEvent = TotalOpenEvent,
                TotalVolunteer = TotalVolunteer,
                TotalPedingVolunteer = TotalPedingVolunteer,
                EventOpenList = EventOpenList,
                TrashEventList = TrashEventList,
                ChartData2 = ChartData2,
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Salestatistic()
        {
            int totalOrderComplete = 0;
            int totalOrderWaiting = 0;
            int totalSales = 0;
            int totalMoneyInMonth = 0;

            MonthlyAnalyticsDto chartData = new();
            CategorySalesDto categorySalesData = new CategorySalesDto();
            List<BestSellingProductDTO> bestSelling = new();
            List<ProductDTO> outOfStockProducts = new();

            ResponseDTO? responseOrderComplete = await _orderService.TotalOrdersComplete();
            if (responseOrderComplete != null && responseOrderComplete.IsSuccess)
            {
                totalOrderComplete = Convert.ToInt32(responseOrderComplete.Result);
            }
            else
            {
                TempData["error"] = responseOrderComplete.Message == null ? "Error" : responseOrderComplete.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseOrderWaiting = await _orderService.TotalOrdersWaiting();
            if (responseOrderWaiting != null && responseOrderWaiting.IsSuccess)
            {
                totalOrderWaiting = Convert.ToInt32(responseOrderWaiting.Result);
            }
            else
            {
                TempData["error"] = responseOrderWaiting.Message == null ? "Error" : responseOrderWaiting.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseTotalSales = await _orderService.TotalSales();
            if (responseTotalSales != null && responseTotalSales.IsSuccess)
            {
                totalSales = Convert.ToInt32(responseTotalSales.Result);
            }
            else
            {
                TempData["error"] = responseTotalSales.Message == null ? "Error" : responseTotalSales.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseTotalMoney = await _orderService.GetTotalMoneyByMonth();
            if (responseTotalMoney != null && responseTotalMoney.IsSuccess)
            {
                totalMoneyInMonth = Convert.ToInt32(responseTotalMoney.Result);
            }
            else
            {
                TempData["error"] = responseTotalMoney.Message == null ? "Error" : responseTotalMoney.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseChart = await _adminService.GetMonthlyAnalytics();
            if (responseChart != null && responseChart.IsSuccess)
            {
                chartData = JsonConvert.DeserializeObject<MonthlyAnalyticsDto>(Convert.ToString(responseChart.Result));
            }
            else
            {
                TempData["error"] = responseChart?.Message ?? "Error fetching chart data";
            }
            ResponseDTO? responseCategorySales = await _adminService.GetSalesByCategory();
            if (responseCategorySales != null && responseCategorySales.IsSuccess)
            {
                categorySalesData = JsonConvert.DeserializeObject<CategorySalesDto>(Convert.ToString(responseCategorySales.Result));
            }
            else
            {
                TempData["error"] = responseCategorySales?.Message ?? "Error fetching category sales data";
            }
            ResponseDTO? responseBestSeller = await _orderService.GetBestSellingProduct();
            if (responseBestSeller != null && responseBestSeller.IsSuccess)
            {
                bestSelling = JsonConvert.DeserializeObject<List<BestSellingProductDTO>>(responseBestSeller.Result.ToString());
            }
            else
            {
                TempData["error"] = responseBestSeller.Message == null ? "Error" : responseBestSeller.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseOutOfStock = await _productservice.OutOfStockProduct();
            if (responseOutOfStock != null && responseOutOfStock.IsSuccess)
            {
                outOfStockProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(responseOutOfStock.Result.ToString());
            }
            else
            {
                TempData["error"] = responseBestSeller.Message == null ? "Error" : responseOutOfStock.Message;
                return RedirectToAction("Index", "Admin");
            }
            var viewModel = new SaleAnalytics
            {
                TotalOrdersComplete = totalOrderComplete,
                TotalOrdersWaiting = totalOrderWaiting,
                TotalSales = totalSales,
                TotalMoneyByMonth = totalMoneyInMonth,
                BestSellingProducts = bestSelling,
                OutOfStockProduct = outOfStockProducts,
                ChartData = chartData,
                CategorySalesChartData = categorySalesData
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Eventstatistic()
        {
            var viewModel = new EventAnalyticViewModel
            {
                TotalEvent = 0,
                TotalOpenEvent = 0,
                TotalVolunteer = 0,
                TotalPedingVolunteer = 0,
                EventOpenList = new List<EventDTO>(),
                TrashEventList = new List<TrashEventDTO>(),
                ChartData = new EventMonthlyAnalyticsDto()
            };

            var responseTotalEvents = await _eventService.GetAllEvent();

            if (responseTotalEvents != null && responseTotalEvents.IsSuccess)
            {
                var eventListJson = responseTotalEvents.Result?.ToString();

                if (!string.IsNullOrEmpty(eventListJson))
                {
                    var eventList = JsonConvert.DeserializeObject<List<EventDTO>>(eventListJson);
                    if (eventList != null)
                    {
                        viewModel.TotalEvent = eventList.Count;
                        viewModel.TotalOpenEvent = eventList.Count(e => e.Status == "Closed");
                    }
                }
            }

            var responseTotalVolunteers = await _volunteerService.GetAllVolunteer();

            if (responseTotalVolunteers != null && responseTotalVolunteers.IsSuccess)
            {
                var volunteerListJson = responseTotalVolunteers.Result?.ToString();

                if (!string.IsNullOrEmpty(volunteerListJson))
                {
                    var volunteerList = JsonConvert.DeserializeObject<List<EventDTO>>(volunteerListJson);
                    if (volunteerList != null)
                    {
                        viewModel.TotalVolunteer = volunteerList.Count;
                        viewModel.TotalPedingVolunteer = volunteerList.Count(v => v.Status == "Pending");
                    }
                }
            }

            ResponseDTO? responseEventOpen = await _eventService.GetOpenEvent();
            if (responseEventOpen != null && responseEventOpen.IsSuccess)
            {
                var eventOpenList = JsonConvert.DeserializeObject<List<EventDTO>>(responseEventOpen.Result.ToString());
                viewModel.EventOpenList = eventOpenList;
            }
            else
            {
                TempData["error"] = responseEventOpen.Message == null ? "Error" : responseEventOpen.Message;
                return RedirectToAction("Index", "Admin");
            }

            ResponseDTO? response = await _trashEventService.GetAllTrashEvent();
            if (response != null && response.IsSuccess)
            {
                var trashEvents = JsonConvert.DeserializeObject<List<TrashEventDTO>>(response.Result.ToString());
                var pendingTrashEvents = trashEvents
                .Where(e => e.Status != null && e.Status.Equals("Chờ xác nhận", StringComparison.OrdinalIgnoreCase))
                .ToList();
                viewModel.TrashEventList = pendingTrashEvents;
            }

            ResponseDTO? responseChart = await _adminService.EventMonthlyAnalytics();
            if (responseChart != null && responseChart.IsSuccess)
            {
                var chartData = JsonConvert.DeserializeObject<EventMonthlyAnalyticsDto>(Convert.ToString(responseChart.Result));
                viewModel.ChartData = chartData;
            }
            else
            {
                TempData["error"] = responseChart?.Message ?? "Error fetching chart data";
            }

            return View(viewModel);
        }

        public async Task<IActionResult> UserList()
        {
            var response = await _userService.GetAllUser();
            if (response != null && response.IsSuccess)
            {
                List<UserDTO> users = response.Result != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(response.Result.ToString()) : new List<UserDTO>();
                return View(users);
            }
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> UserDetail(string userId)
        {
            try
            {
                int userPoints = 0;
                UserDTO user = new();
                RewardPointDTO rewardPoint = new();
                List<PointTransactionDTO> userPointTransactions = new();
                List<VolunteerDTO> participatedActivities = new();
                List<OrderDTO> orderHistory = new();

                ResponseDTO? responseTotalPoints = await _rewardPointService.GetUserTotalPoints(userId);
                if (responseTotalPoints != null && responseTotalPoints.IsSuccess)
                {
                    rewardPoint = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardPointDTO>(responseTotalPoints.Result.ToString());
                    userPoints = (int)rewardPoint.TotalPoints;
                }

                ResponseDTO? response = await _userService.GetUserById(userId);
                if (response != null && response.IsSuccess)
                {
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(response.Result.ToString());
                }
                else
                {
                    TempData["error"] = response?.Message ?? "Error fetching user details";
                }
                ResponseDTO? pointTransactionResponse = await _pointTransactionService.GetUserPointTransactions(userId);
                if (pointTransactionResponse != null && pointTransactionResponse.IsSuccess)
                {
                    userPointTransactions = JsonConvert.DeserializeObject<List<PointTransactionDTO>>(pointTransactionResponse.Result.ToString()) ?? new List<PointTransactionDTO>();
                }
                else
                {
                    TempData["error"] = pointTransactionResponse.Message == null ? "Error" : pointTransactionResponse.Message;
                    return RedirectToAction("UserList", "Admin");
                }
                ResponseDTO? orderResponse = await _orderService.GetOrderByUserID(userId);
                if (orderResponse != null && orderResponse.IsSuccess)
                {
                    orderHistory = JsonConvert.DeserializeObject<List<OrderDTO>>(orderResponse.Result.ToString()) ?? new List<OrderDTO>();
                }
                else
                {
                    TempData["error"] = pointTransactionResponse.Message == null ? "Error" : pointTransactionResponse.Message;
                }
                ResponseDTO? participatedResponse = await _volunteerService.GetParticipatedActivitiesByUserId(userId);
                if (participatedResponse != null && participatedResponse.IsSuccess)
                {
                    participatedActivities = JsonConvert.DeserializeObject<List<VolunteerDTO>>(participatedResponse.Result.ToString()) ?? new List<VolunteerDTO>();
                }
                else
                {
                    TempData["error"] = participatedResponse.Message == null ? "Error" : participatedResponse.Message;
                }
                var userProfileViewModel = new UserProfileViewModel
                {
                    TotalPoints = userPoints,
                    User = user,
                    PointTransactions = userPointTransactions,
                    OrderHistory = orderHistory,
                    ParticipatedActivities = participatedActivities
                };

                return View(userProfileViewModel);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UserList", "Admin");
            }
        }

        public async Task<IActionResult> BlockUser(string userId)
        {
            try
            {
                var response = await _userService.BanUser(userId);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Chặn user thành công";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction("UserList", "Admin");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UserList", "Admin");
            }
        }

        public async Task<IActionResult> UnblockUser(string userId)
        {
            try
            {
                var response = await _userService.UnBanUser(userId);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Mở chặn user thành công";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction("UserList", "Admin");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("UserList", "Admin");
            }
        }

        public async Task<IActionResult> LogStaff()
        {
            var response = await _adminService.GetAllLog();
            if (response != null && response.IsSuccess)
            {
                var rawLogs = System.Text.Json.JsonSerializer.Deserialize<List<SystemLogDTO>>(response.Result.ToString(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var viewModel = rawLogs.Select(log => new SystemLogViewModel
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    ActionType = log.ActionType,
                    ObjectType = log.ObjectType,
                    ObjectId = log.ObjectId,
                    Description = log.Description,
                    CreatedAt = log.CreatedAt,
                    FullName = log.User?.FullName,
                    Address = log.User?.Address,
                    Avatar = log.User?.Avatar ?? "default.png",
                    UserName = log.User?.UserName,
                    Email = log.User?.Email,
                    IsBanned = log.User?.LockoutEnd != null && log.User.LockoutEnd > DateTimeOffset.UtcNow
                }).ToList();

                return View(viewModel);
            }
            return RedirectToAction("Index", "Admin");
        }

    }
}
