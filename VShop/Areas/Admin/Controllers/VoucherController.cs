using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VShop.BLL.ServiceContracts;
using VShop.BLL.Services;
using VShop.DAL.Models.Db;

namespace VShop.Areas.Admin.Controllers
{
    [Area("admin")]
    public class VoucherController : Controller
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public async Task<IActionResult> Index(string? search, bool? status, DateTime? start, DateTime? end, int page = 1)
        {
            int pageSize = 6;
            var list = await _voucherService.GetAllVoucherAsync(search, status,start,end);
            ViewData["list"] = list;
            var num = list.ToList().Count;
            var count = Math.Ceiling((decimal)num / pageSize);
            var listInPage = list.Skip((page - 1) * pageSize)
                               .Take(pageSize).ToList();
            ViewData["list"] = listInPage;
            ViewData["count"] = count;
            ViewData["search"] = search;
            ViewData["status"] = status;
            ViewData["start"] = start;
            ViewData["end"] = end;
            ViewData["page"] = page;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Voucher voucher)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            voucher.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
            voucher.CreatedDate = DateTime.Now;
            await _voucherService.Create(voucher);
            return Redirect("/Admin/Voucher");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Voucher voucher)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _voucherService.Update(voucher);
            return Redirect("/Admin/Voucher");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _voucherService.Delete(id);
            return Redirect("/Admin/Voucher");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            await _voucherService.ChangeStatus(id);
            return Redirect("/admin/Voucher");
        }

    }
}
