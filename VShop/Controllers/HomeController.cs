using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VShop.Models;
using VShop.BLL.ServiceContracts;

namespace VShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, IPostService postService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            //--best selling items--
            //get list best selling items of all category
            var listBestSellingItems = await _productService.GetBestSellingProductsAsync();
            ViewData["ListBestSellingItems"] = listBestSellingItems;
            //get list category that has product sold
            var listCategory = await _categoryService.GetListNameCategorySoldAsync();
            ViewData["ListCategorySold"] = listCategory;

            // get list best selling items of each category
            for (int i = 0;i < listCategory.Count; i++)
            {
                ViewData["ListBestSellingItems" + (i + 1)] = await _productService.GetBestSellingProductsAsync(listCategory[i]);
            }

            //--new arrivals -- 
            // get list new arrivals of all category
            var listNewArrivals = await _productService.GetNewArrivalsAsync();
            ViewData["ListNewArrivals"] = listNewArrivals;
            // get list category has product
            var listCategoryHasProduct = await _categoryService.GetListNameCategoryHasProductAsync();
            ViewData["ListCategoryHasProduct"] = listCategoryHasProduct;
            //get list new arrival products of each category
            for (int i = 0; i < listCategoryHasProduct.Count; i++)
            {
                ViewData["ListNewArrivals" + (i + 1)] = await _productService.GetNewArrivalsAsync(listCategoryHasProduct[i]);
            }

            //get 3 ActivePost;
            var listPost = await _postService.GetAllPostAsync();
            ViewData["ListPost"] = listPost.OrderByDescending( x=> x.CreateAt).Take(3).ToList();

            //return
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
