using Microsoft.AspNetCore.Mvc;
using VShop.DAL.Enums;
using VShop.BLL.ServiceContracts;

namespace VShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ICategoryService categoryService,ICommentService commentService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _commentService = commentService;
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

            var listProducts = await _productService.GetAllActiveProductsAsync(search,category,minPrice,maxPrice,sortBy,isDescending,page,9);
            return View(listProducts);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) 
            {
                return NotFound();
            }
            var listNewArrivals = await _productService.GetNewArrivalsAsync(product.CategoryName);
            ViewData["ListNewArrivals"] = listNewArrivals;
            var comments = _commentService.GetAllCommentOfProduct(id);
            ViewData["comments"] = comments;
            return View(product);
        }
    }
}
