using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;

namespace VShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] CartCookieDTO request)
        {
            bool isProductExist = await _productService.CheckProductExist(request.ProductId);
            if (!isProductExist)
            {
                return NotFound();
            }

            var cartItems = GetCart();
            var foundProductInCart = cartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
            if(foundProductInCart == null)
            {
                var newItem =  new CartCookieDTO
                {
                    ProductId = request.ProductId,
                    Count = request.Count,
                };

                cartItems.Add(newItem);
            }
            else
            {
                if(request.Count > 0)
                {
                    foundProductInCart.Count += request.Count;
                }
                else
                {
                    cartItems.Remove(foundProductInCart);
                }
            }

            var json = JsonConvert.SerializeObject(cartItems);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("Cart", json, option);

            var result = cartItems.Sum(x => x.Count);

            return Ok(result);
        }

        public async Task<IActionResult> SmallCart()
        {
            var cartItems = GetCart();
            var list = await _productService.GetProductsInCart(cartItems) ;
            return PartialView(list);
        }

        public List<CartCookieDTO> GetCart()
        {
            List<CartCookieDTO> cartList = new List<CartCookieDTO>();
            var preCartItemsString = Request.Cookies["Cart"];
            if (!string.IsNullOrEmpty(preCartItemsString))
            {
                cartList = JsonConvert.DeserializeObject<List<CartCookieDTO>>(preCartItemsString);
            }
            return cartList;
        }

        public IActionResult Clear()
        {
            Response.Cookies.Delete("Cart");
            return RedirectToAction("Index");
        }
    }
}
