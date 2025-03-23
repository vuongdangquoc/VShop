using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;
using VShop.BLL.DTO;
using VShop.BLL.Helper;
using VShop.BLL.ServiceContracts;
using VShop.BLL.VNPay.Models;
using VShop.BLL.VNPay.Service;
using VShop.DAL.Models.Db;

namespace VShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vpnPayService;
        public CartController(IProductService productService, IOrderService orderService, IVnPayService vnPayService)
        {
            _productService = productService;
            _orderService = orderService;
            _vpnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = GetCart();
            var list = await _productService.GetProductsInCart(cartItems);
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] CartCookieDTO request,[FromQuery] bool? updateQuantity = null)
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
                    if(updateQuantity != null)
                    {
                        foundProductInCart.Count = request.Count;
                    }
                    else
                    {
                        foundProductInCart.Count += request.Count;
                    }
                    
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var cartItems = GetCart();
            var list = await _productService.GetProductsInCart(cartItems);
            ViewData["products"] = list;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderDTO orderDTO)
        {
            if (ModelState.IsValid)
            {
                var cartItems = GetCart();
                var products = await _productService.GetProductsInCart(cartItems);
                orderDTO.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _orderService.Checkout(orderDTO, products);

                if (result == "VNPay")
                {
                    var paymentInfo = new PaymentInformationModel
                    {
                        Name = User.FindFirst(ClaimTypes.Name)?.Value!,
                        Amount = products.Sum(x => x.SumPrice).Value + 20000,
                        OrderDescription = "Thanh toan don hang VSHOP",
                        OrderType = "other"
                    };
                    var url = _vpnPayService.CreatePaymentUrl(paymentInfo, HttpContext);
                    return Redirect(url);
                }
                else if (result == "Thanh toán khi nhận hàng")
                {
                    return RedirectToAction("OrderSuccess", "Cart");
                }
            }           

            return RedirectToAction("Checkout","Cart");
        }

        [Authorize]
        public async Task<IActionResult> Result()
        {
            string vnp_ResponseCode = Request.Query["vnp_ResponseCode"];
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _orderService.CheckPaymentSuccess(userId,vnp_ResponseCode);
            if (result)
            {
                return RedirectToAction("OrderSuccess");
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize]

        public async Task<IActionResult> OrderSuccess()
        {
            Response.Cookies.Delete("Cart");

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var order = await _orderService.GetNewByUserID(userId);

            // Gửi đơn hàng đến email của khách hàng khi vừa đặt hàng thành công
            var html = "";
            double totalMoney = 0;

            foreach (var item in order.OrderDetails)
            {
                html += "<tr>";

                var productName = item.Product.Name;
                var quantity = item.Quantity;
                if (item.Product.Discount > 0)
                {
                    var price = item.Product.Price - (item.Product.Price * item.Product.Discount / 100);
                    totalMoney += price * quantity;

                    html += "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + productName + "</td>"
                    + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + price.ToString("C0", new CultureInfo("vi-VN")) + "</td>\r\n"
                    + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + quantity + "</td>\r\n"
                    + "<td style=\"border: 1px solid black; padding: 8px; text-align: right;\">" + (price * quantity).ToString("C0", new CultureInfo("vi-VN")) + "</td>";
                }
                else
                {
                    var price = item.Product.Price;
                    totalMoney += price * quantity;

                    html += "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + productName + "</td>"
                   + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + price.ToString("C0", new CultureInfo("vi-VN")) + "</td>\r\n"
                   + "<td style=\"border: 1px solid black; padding: 8px; text-align: center;\">" + quantity + "</td>\r\n"
                   + "<td style=\"border: 1px solid black; padding: 8px; text-align: right;\">" + (price * quantity).ToString("C0", new CultureInfo("vi-VN")) + "</td>";
                }

                html += "</tr>";
            }

            double totalPayment = totalMoney + order.DeliveryFee;

            string template = System.IO.File.ReadAllText("wwwroot/assets/customer/template/order.html");

            template = template.Replace("{{orderShopping}}", html);
            template = template.Replace("{{totalMoney}}", totalMoney.ToString("C0", new CultureInfo("vi-VN")));
            template = template.Replace("{{deliveryFee}}", order.DeliveryFee.ToString("C0", new CultureInfo("vi-VN")));
            double voucherValue = 0;
            if (order.Voucher != null)
            {
                if (order.Voucher.IsDiscountPercentage != true)
                {
                    voucherValue = order.Voucher.DiscountValue;
                }
                else
                {
                    voucherValue = totalMoney * order.Voucher.DiscountValue / 100;
                }

                template = template.Replace("{{voucher}}", voucherValue.ToString("C0", new CultureInfo("vi-VN")));
                totalPayment -= voucherValue;
            }
            else
            {
                template = template.Replace("{{voucher}}", "0");
            }
            template = template.Replace("{{totalPayment}}", totalPayment.ToString("C0", new CultureInfo("vi-VN")));

            template = template.Replace("{{fullName}}", order.FullName);
            template = template.Replace("{{email}}", order.Email);
            template = template.Replace("{{phoneNumber}}", order.PhoneNumber);

            template = template.Replace("{{province}}", order.ProvinceName.ToString());
            template = template.Replace("{{district}}", order.DistrictName.ToString());
            template = template.Replace("{{ward}}", order.WardName.ToString());
            template = template.Replace("{{address}}", order.Address);
            template = template.Replace("{{note}}", order.Note);

            await EmailService.SendMailAsync("FashionShop", "Đặt hàng thành công", template, userEmail).ConfigureAwait(false);

            return View(order);
        }
    }
}
