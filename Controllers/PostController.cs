using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationY.Controllers
{
    public class PostController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IPost _postRepository;

        public PostController(Context context, IPost postRepository, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> CreatePost()
        {
            if(User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if(UserInfo != null)
                {
                    ViewBag.UserInfo = UserInfo;
                    return View();
                }
            }
            return RedirectToAction("Create", "Account");
        }
    }
}
