using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VShop.BLL.ServiceContracts;
using VShop.BLL.Services;
using VShop.DAL.Enums;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string? search, bool? status, int page =1)
        {
            int pageSize = 6;
            var list = await _categoryService.GetAllCategoryAsync(search,status);
            ViewData["list"] = list;
            var num = list.ToList().Count;
            var count = Math.Ceiling((decimal)num / pageSize);
            var listInPage = list.Skip((page - 1) * pageSize)
                               .Take(pageSize).ToList();
            ViewData["list"] = listInPage;
            ViewData["count"] = count;
            ViewData["search"] = search;
            ViewData["status"] = status;
            ViewData["page"] = page;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryService.Create(category);
            return Redirect("/Admin/Category");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryService.Update(category);
            return Redirect("/Admin/Category");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Delete(id);
            return Redirect("/Admin/Category");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            await _categoryService.ChangeStatus(id);
            return Redirect("/admin/category");
        }
    }
}
