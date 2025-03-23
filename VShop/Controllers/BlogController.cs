using Microsoft.AspNetCore.Mvc;
using VShop.BLL.ServiceContracts;

namespace VShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostService _postService;
        public BlogController(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IActionResult> Index()
        {
            var listPost = await _postService.GetActivePostsAsync();

            return View(listPost);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return View(post);
        }
    }
}
