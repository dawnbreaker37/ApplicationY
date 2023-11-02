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
            List<GetProjects_ViewModel>? RandomProjects = null;
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null) ViewBag.UserInfo = UserInfo;

                IQueryable<GetProjects_ViewModel>? RandomProjects_Preivew = _projectRepository.GetRandomProjects(6);
                if(RandomProjects_Preivew != null)
                {
                    RandomProjects = await RandomProjects_Preivew.ToListAsync();
                    Count = RandomProjects.Count;
                }
            }
            ViewBag.UserInfo = null;
            ViewBag.Projects = RandomProjects;
            ViewBag.ProjectsCount = Count;

            return View();
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