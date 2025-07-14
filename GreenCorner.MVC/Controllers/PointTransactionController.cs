using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class PointTransactionController : Controller
    {
        private readonly IPointTransactionService _pointTransactionService;
        private readonly IUserService _userService;

        public PointTransactionController(IPointTransactionService pointTransactionService, IUserService userService)
        {
            _pointTransactionService = pointTransactionService;
            _userService = userService;
        }
        public async Task<IActionResult> PointTransaction()
        {
            return View();
        }
        public async Task<IActionResult> PointTransactionList()
        {
            List<PointTransactionListViewModel> viewModelList = new();
            ResponseDTO? response = await _pointTransactionService.GetAllPointTransactions();
            if (response != null && response.IsSuccess)
            {
                var pointTransactions = JsonConvert.DeserializeObject<List<PointTransactionDTO>>(response.Result.ToString());
                foreach (var pointTransaction in pointTransactions)
                {
                    ResponseDTO? userResponse = await _userService.GetUserById(pointTransaction.UserId);

                    UserDTO userDTO = new UserDTO();

                    if (userResponse != null && userResponse.IsSuccess)
                    {
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    viewModelList.Add(new PointTransactionListViewModel
                    {
                        pointTransactionDTO = pointTransaction,
                        User = userDTO
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> PointTransaction(string userId, int exchangePoint)
        {
            var response = await _pointTransactionService.ExchangePoints(userId, exchangePoint);
            if (response != null && response.IsSuccess)
                return RedirectToAction("Index","RewardPoint");

            return View("Error"); 
        }

    
        public async Task<IActionResult> ViewTransactions(string userId)
        {
            var response = await _pointTransactionService.GetExchangeTransactions(userId);
            return View(response);
        }
    }

}
