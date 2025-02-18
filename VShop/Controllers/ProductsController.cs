using Microsoft.AspNetCore.Mvc;
using VShop.DAL.Enums;
using VShop.BLL.ServiceContracts;

namespace VShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(string search,
             string? category,
             double? minPrice,
             double? maxPrice,
             SortBy sortBy,
             bool isDescending,
             int page =1)
        {

            ViewData["page"] = page;
            ViewData["category"] = category;
            ViewData["search"] = search;
            ViewData["sortBy"] = sortBy;
            ViewData["isDescending"] = isDescending;

            var listCategoryViewModel = await _categoryService.GetCategoryViewModelsAsync();
            ViewData["listCategoryViewModel"] = listCategoryViewModel;

            var listNewArrivals = await _productService.GetNewArrivalsAsync();
            ViewData["ListNewArrivals"] = listNewArrivals;

            var listProducts = await _productService.GetAllProductsAsync(search,category,minPrice,maxPrice,sortBy,isDescending,page,9);
            return View(listProducts);
        }
    }
}
