using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;

namespace VShop.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IActionResult> Index(string? search, bool? status, int page = 1)
        {
            int pageSize = 6;
            var list = await _postService.GetAllCategoryAsync(search, status);
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
        public async Task<IActionResult> Add(Post post, IFormFile file)
        {

            post.CreateBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            post.CreateAt = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Redirect("/Admin/Post");
            }
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string baseDirectory = "UploadFiles\\Posts";
            string folder = Path.Combine(webRootPath, baseDirectory);
            if (file != null)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(folder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                post.Image = (baseDirectory + "\\" + uniqueFileName).Replace("\\", "/");
            }
            await _postService.Create(post);
            return Redirect("/Admin/Post");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string baseDirectory = "UploadFiles\\Posts";
            string folder = Path.Combine(webRootPath, baseDirectory);
            if (file != null)
            {
                var x = await _postService.GetPostByIdAsync(post.Id);
                if(x != null)
                {
                    var oldImagePath = Path.Combine(webRootPath,x.Image.Replace("\\", "/"));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                
                string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(folder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                post.Image = (baseDirectory + "\\" + uniqueFileName).Replace("\\", "/");
            }
            await _postService.Update(post);
            return Redirect("/Admin/Post");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.Delete(id);
            return Redirect("/Admin/Post");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var x = await _postService.GetPostByIdAsync(id);
            if (x != null)
            {
                var oldImagePath = Path.Combine(webRootPath, x.Image.Replace("\\", "/"));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            await _postService.ChangeStatus(id);
            return Redirect("/admin/Post");
        }
    }
}
