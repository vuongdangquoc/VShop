using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;

namespace VShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        public INotyfService _notifyService { get; }

        public ContactController(IContactService contactService, INotyfService notyfService)
        {
            _contactService = contactService;
            _notifyService = notyfService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(AddContactDTO contact)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = await _contactService.AddContactAsync(contact);
                if (isSuccess)
                {
                    _notifyService.Success("Gửi thành công");
                    return View(contact);
                }
                return RedirectToAction("Index");

            }
            else
            {

                return View(contact);
            }
        }
    }

}
