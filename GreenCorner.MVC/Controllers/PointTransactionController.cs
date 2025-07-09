using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class PointTransactionController : Controller
    {
        private readonly IPointTransactionService _pointTransactionService;

        public PointTransactionController(IPointTransactionService pointTransactionService)
        {
            _pointTransactionService = pointTransactionService;
        }
        public async Task<IActionResult> PointTransaction()
        {
            return View();
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
