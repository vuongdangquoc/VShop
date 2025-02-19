using Microsoft.AspNetCore.Mvc;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;

namespace VShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }
            var isSuccess = await _userService.Login(account);
            if(isSuccess == false)
            {
                TempData["ErrorMessage"] = "Sorry we couldn't find an account. Please try again!";
                return View(account);
            }
            return View(account);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO account)
        {
            return View();
        }
    }
}
