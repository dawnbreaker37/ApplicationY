using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

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

        public async Task<IActionResult> Edit(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                { 
                    Project? ProjectInfo = await _projectRepository.GetProjectAsync(Id, true);
                    if (ProjectInfo != null && ProjectInfo.UserId == UserInfo.Id)
                    {
                        string? MainFullText = null;
                        StringBuilder? FullText = new StringBuilder(ProjectInfo.TextPart1);
                        if (!String.IsNullOrEmpty(ProjectInfo.TextPart2)) FullText = FullText?.Append(ProjectInfo.TextPart2);
                        if (!String.IsNullOrEmpty(ProjectInfo.TextPart3)) FullText = FullText?.Append(ProjectInfo.TextPart3);

                        double PercentageOfTargetPrice = Math.Round((double)(ProjectInfo.TargetPrice / 10000000 * 100), 1);
                        if (FullText != null) MainFullText = FullText?.ToString();

                        ViewBag.UserInfo = UserInfo;
                        ViewBag.ProjectInfo = ProjectInfo;
                        ViewBag.FullText = MainFullText;
                        ViewBag.FullTextLength = MainFullText?.Length;
                        ViewBag.PercentageOfTargetPrice = PercentageOfTargetPrice;

                        return View();
                    }
                    else return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> EditProject(CreateProject_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                string? Result = await _projectRepository.EditAsync(Model);
                if (Result != null) return Json(new { success = true, alert = "The " + Result + " project has successfully updated. Go by link below to check the updated project", link = Model.Id });
                else return Json(new { success = false, alert = "An error occured while trying to update project information. Please, check all entered datas and then try again" });
            }
            return Json(new { success = false, alert = "Something from entered datas is wrong. Please, check them all and then try again" });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id, int UserId)
        {
            int Result = await _projectRepository.RemoveAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, Id = Result, alert = "Selected project has successfully removed" });
            else return Json(new { success = false, alert = "Unable to remove selected project at this moment. Please, try again later" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFullProject(int Id, bool GetAdditionalInfo)
        {
            Project? ProjectInfo = await _projectRepository.GetProjectAsync(Id, GetAdditionalInfo);
            if (ProjectInfo != null)
            {
                return Json(new { success = true, result = ProjectInfo });
            }
            else return Json(new { success = false, alert = "Unable to have a look on this project at this moment. Please, try again later" });
        }

        public async Task<IActionResult> Info(int Id)
        {
            if(Id != 0)
            {
                Project? ProjectInfo = await _projectRepository.GetProjectAsync(Id, true);
                if(ProjectInfo != null)
                {
                    double PreviousPricePercentage = 0;

                    StringBuilder sb = new StringBuilder();
                    sb.Append(ProjectInfo.TextPart1);
                    if (ProjectInfo.TextPart2 != null) sb.Append(ProjectInfo.TextPart2);
                    if(ProjectInfo.TextPart3 != null) sb.Append(ProjectInfo.TextPart3);

                    if (ProjectInfo.PastTargetPrice != 0)
                    {
                        PreviousPricePercentage = (double)ProjectInfo.TargetPrice / ProjectInfo.PastTargetPrice * 100;
                        if (PreviousPricePercentage < 100) PreviousPricePercentage = Math.Round(100 - PreviousPricePercentage, 1) * -1;
                        else PreviousPricePercentage = Math.Round(PreviousPricePercentage - 100, 1);
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        User? UserInfo = await _userManager.GetUserAsync(User);
                        if (UserInfo != null) ViewBag.UserInfo = UserInfo;
                        else ViewBag.UserInfo = null;
                    }
                    ViewBag.ProjectInfo = ProjectInfo;
                    ViewBag.TargetPriceChangePerc = PreviousPricePercentage;
                    ViewBag.FullText = sb;

                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
