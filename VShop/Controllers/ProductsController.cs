using Microsoft.AspNetCore.Mvc;
using VShop.Models.Db;

namespace VShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly VShopContext _context;

        public ProductsController( VShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public List<Product> GetBestSellingItems()
        {

        }
    }
}
