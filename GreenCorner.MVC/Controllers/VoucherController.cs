using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public async Task<IActionResult> Index()
        {
            List<VoucherDTO> listVoucher = new();
            ResponseDTO? response = await _voucherService.GetAllVoucher();
            if (response != null && response.IsSuccess)
            {
                listVoucher = JsonConvert.DeserializeObject<List<VoucherDTO>>(response.Result.ToString());
            }
            return View(listVoucher);
        }
        public async Task<IActionResult> Detail(int voucherId)
        {
            VoucherDTO voucher = new();
            ResponseDTO? response = await _voucherService.GetVoucherById(voucherId);

            if (response != null && response.IsSuccess)
            {
                voucher = JsonConvert.DeserializeObject<VoucherDTO>(response.Result.ToString());
            }
            else
            {
                return NotFound(); // hoặc RedirectToAction("Index") nếu bạn muốn
            }

            return View("Detail", voucher); // View tên Detail.cshtml
        }

        public IActionResult Create()
        {
            return View(new VoucherDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create(VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(voucherDTO);
            }

            var response = await _voucherService.AddVoucher(voucherDTO);
            if (response != null && response.IsSuccess)
                return RedirectToAction(nameof(Index));

            return View(voucherDTO);
        }

        public async Task<IActionResult> Edit(int voucherId)
        {
            ResponseDTO response = await _voucherService.GetVoucherById(voucherId);
            if (response != null && response.IsSuccess)
            {
                VoucherDTO voucher = JsonConvert.DeserializeObject<VoucherDTO>(response.Result.ToString());
                return View(voucher);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VoucherDTO voucherDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(voucherDTO);
            }

            var response = await _voucherService.UpdateVoucher(voucherDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Voucher updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = response?.Message;
            return View(voucherDTO);
        }

        public async Task<IActionResult> Delete(int voucherId)
        {
            ResponseDTO response = await _voucherService.GetVoucherById(voucherId);
            if (response != null && response.IsSuccess)
            {
                VoucherDTO voucher = JsonConvert.DeserializeObject<VoucherDTO>(response.Result.ToString());
                return View(voucher);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VoucherDTO voucherDTO)
        {
            ResponseDTO response = await _voucherService.DeleteVoucher(voucherDTO.VoucherId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Voucher deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(voucherDTO);
        }
    }


}
