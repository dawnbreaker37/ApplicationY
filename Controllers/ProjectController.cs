using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class ProjectController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IProject _projectRepository;
        private readonly Context _context;

        public ProjectController(UserManager<User> userManager, IProject projectRepository, Context context)
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                ViewBag.UserInfo = UserInfo;

                return View();
            }
            return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProject_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo?.Id == Model.UserId)
                {
                    string? Result = await _projectRepository.CreateAsync(Model);
                    if (Result != null)
                    {
                        int LastProjectId = await _projectRepository.GetUsersLastProjectIdAsync(Model.UserId);
                        return Json(new { success = true, name = Result, link = LastProjectId, alert = "Project " + Result + " has been successfully created. Go by link below to have a look on it" });
                    }
                    else return Json(new { success = false, alert = "An error occured while trying to create your project. Please, check all entered datas and be sure that there's no missentered or empty values and then try again" });
                }
                else return Json(new { success = false, alert = "Please, check all datas and be sure that you've entered them correctly" });
            }
            else return Json(new { success = false, alert = "Please, log in or sign in to create a project" });
        }

        public async Task<IActionResult> All()
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                {
                    IQueryable<Project?> UserAllProjects_Preview = _projectRepository.GetUsersAllProjectsAsync(UserInfo.Id, false);
                    if (UserAllProjects_Preview != null)
                    {
                        List<Project?> Projects = await UserAllProjects_Preview.ToListAsync();
                        int Count = Projects.Count;

                        ViewBag.UserInfo = UserInfo;
                        ViewBag.Projects = Projects;
                        ViewBag.Count = Count;

                        return View();
                    }
                }
            }
            else return RedirectToAction("Create", "Account");
            return RedirectToAction("Index", "Home");
        }
    }
}
