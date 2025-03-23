using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using VShop.DAL.Models.Db;

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
            var userDTO = await _userService.Login(account);
            if(userDTO == null)
            {
                ModelState.AddModelError("","Login failed!! Please check again!");
                return View(account);
            }
            
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userDTO.FullName));
            claims.Add(new Claim(ClaimTypes.Email, userDTO.Email));
            //-----------
            claims.Add(new Claim(ClaimTypes.Role, userDTO.Role));
            claims.Add(new Claim("Avatar", userDTO.Image));
            //-----------
            //Create an identity based on the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //-----------
            //Create a principal based on identity
            var principal = new ClaimsPrincipal(identity);
            //-----------
            //Sign in the user with the created princical
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("Cart");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO account)
        {
            if (String.IsNullOrEmpty(account.FullName?.Trim()))
            {
                account.FullName = account.Email?.Trim();
            }
            else
            {
                account.FullName = account.FullName?.Trim();
            }
            
            account.Email = account.Email?.Trim();
            account.Password = account.Password?.Trim();
            account.Phone = account.Phone?.Trim();
            account.ConfirmPassword = account.ConfirmPassword?.Trim();
            //-----------------
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            //-----------Duplicate Email Checking --------------------
            var isEmailExist = await _userService.CheckEmailExistAsync(account.Email);
            if (isEmailExist)
            {
                ModelState.AddModelError("Email", "Email is used");
                return View(account);
            }
            //-----------Duplicate Phone Checking --------------------
            var isPhoneExist = await _userService.CheckPhoneExistAsync(account.Phone);
            if (isPhoneExist)
            {
                ModelState.AddModelError("Phone", "Phone is used");
                return View(account);
            }

            var result = await _userService.Register(account);
            if(result == false)
            {
                return View(account);
            }

            return RedirectToAction("login");
        }
    }
}
