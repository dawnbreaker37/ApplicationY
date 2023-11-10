using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ApplicationY.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUser _user;
        private readonly ILogger<HomeController> _logger;
        private readonly IProject _projectRepository;

        public HomeController(ILogger<HomeController> logger, IUser user, IProject projectRepository, UserManager<User> userManager)
        {
            _user = user;
            _logger = logger;
            _userManager = userManager;
            _projectRepository = projectRepository;
        }

        public async Task<IActionResult> Index()
        {
            User? UserInfo = await _userManager.GetUserAsync(User);
            if (UserInfo != null)
            {
                ViewBag.UserInfo = UserInfo;
            }

            return View();
        }

        public async Task<IActionResult> Search()
        {
            int Count = 0;
            int UsersCount = 0;
            List<GetProjects_ViewModel>? RandomProjects = null;
            List<GetUserInfo_ViewModel>? RandomUsers = null;
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null) ViewBag.UserInfo = UserInfo;

                IQueryable<GetUserInfo_ViewModel>? RandomUsers_Preview = _user.GetRandomUsers(12);
                IQueryable<GetProjects_ViewModel>? RandomProjects_Preivew = _projectRepository.GetRandomProjects(6);
                if(RandomProjects_Preivew != null)
                {
                    RandomProjects = await RandomProjects_Preivew.ToListAsync();
                    Count = RandomProjects.Count;
                }
                if(RandomProjects_Preivew != null)
                {
                    RandomUsers = await RandomUsers_Preview.ToListAsync();
                    UsersCount = RandomUsers.Count;
                }
            }
            ViewBag.UserInfo = null;
            ViewBag.Projects = RandomProjects;
            ViewBag.ProjectsCount = Count;
            ViewBag.UsersCount = UsersCount;   
            ViewBag.Users = RandomUsers;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchEverywhere(string Keyword, int MinPrice, int MaxPrice, bool SearchOnlyProjects, bool SearchOnlyUsers)
        {
            int ProjectsCount = 0;
            int UsersCount = 0;
            IQueryable<GetProjects_ViewModel>? ProjectsResult_Preview = null;
            IQueryable<GetUserInfo_ViewModel>? UsersResult_Preview = null;
            List<GetUserInfo_ViewModel>? UsersResult = null;
            List<GetProjects_ViewModel>? ProjectsResult = null;
            if (SearchOnlyProjects && !SearchOnlyUsers) ProjectsResult_Preview = _projectRepository.FindProjects(Keyword, MinPrice, MaxPrice, 0);
            else if(!SearchOnlyProjects && SearchOnlyUsers) UsersResult_Preview = _user.FindUsers(Keyword);
            else
            {
                UsersResult_Preview = _user.FindUsers(Keyword);
                ProjectsResult_Preview = _projectRepository.FindProjects(Keyword, MinPrice, MaxPrice, 0);
            }

            if(UsersResult_Preview != null)
            {
                UsersResult = await UsersResult_Preview.ToListAsync();
                UsersCount = UsersResult.Count;
            }
            if(ProjectsResult_Preview != null)
            {
                ProjectsResult = await ProjectsResult_Preview.ToListAsync();  
                ProjectsCount = ProjectsResult.Count;
            }

            return Json(new { success = true, userResult = UsersResult, projectResult = ProjectsResult, usersCount = UsersCount, projectsCount = ProjectsCount });           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}