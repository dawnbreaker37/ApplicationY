using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class PostController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IPost _postRepository;
        private readonly IProject _projectRepository;

        public PostController(Context context, IPost postRepository, IProject projectRepository, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _postRepository = postRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IActionResult> CreatePost()
        {
            if(User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if(UserInfo != null)
                {
                    List<Project?>? Projects = null;
                    IQueryable<Project?>? Projects_Preview = _projectRepository.GetUsersAllProjectsWInfo(UserInfo.Id, UserInfo.Id, false);
                    if(Projects_Preview != null)
                    {
                        Projects = Projects_Preview.ToList();
                    }

                    ViewBag.ProjectsCount = Projects?.Count;
                    ViewBag.Projects = Projects;
                    ViewBag.UserInfo = UserInfo;
                    return View();
                }
            }
            return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePost_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                int Result = await _postRepository.CreatePostAsync(Model);
                if (Result != 0) return Json(new { success = true, id = Result, alert = "Your post has been successfully... posted" });
                else return Json(new { success = false, alert = "Unable to post it. Check the text of your post, may be it's too large for this kind of content" });
            }
            else return Json(new { success = false, alert = "An error occured while trying to post it. Please, chec all datas and try again" });
        }

        [HttpPost]
        public async Task<IActionResult> Like(int Id, int UserId)
        {
            int Result = await _postRepository.LikeThePostAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, id = Id });
            else return Json(new { success = false, alert = "Unable to like that post. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int Id, int UserId)
        {
            int Result = await _postRepository.DislikeThePostAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, id = Id });
            else return Json(new { success = false, alert = "Unable to remove your like from that post. Please, try again later" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRelatedPosts(int Id)
        {
            IQueryable<GetPost_ViewModel>? Posts_Preview = _postRepository.GetAllAssociatedPostsWAdditionalInfo(Id);
            if(Posts_Preview != null)
            {
                List<GetPost_ViewModel>? Posts_Result = await Posts_Preview.ToListAsync();
                if (Posts_Result != null) return Json(new { success = true, count = Posts_Result.Count, result = Posts_Result });
            }
            return Json(new { success = false, alert = "We haven't found any post related with this project" });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id, int UserId)
        {
            int Result = await _postRepository.RemovePostAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, alert = "The selected post was successfully deleted", result = Id });
            else return Json(new { success = false, alert = "Unable to remove that post. Please, try again later" });
        }
    }
}
