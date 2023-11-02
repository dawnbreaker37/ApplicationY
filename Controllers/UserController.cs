using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class UserController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUser _userRepository;
        private readonly IAccount _accountRepository;
        private readonly IProject _projectRepository;

        public UserController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IAccount accountRepository, IUser userRepository, IProject projectRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IActionResult> Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                {
                    UserInfo.Country = await _context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == UserInfo.CountryId);

                    double ProfileFullnessPercentage = 0;
                    TimeSpan DaysTillLastChange = DateTime.Now.Subtract(UserInfo.PasswordResetDate.Date);
                    bool PermissionToChangePassword;
                    if (DaysTillLastChange.TotalDays >= 28) PermissionToChangePassword = true;
                    else PermissionToChangePassword = false;

                    if (UserInfo.Description != null) ProfileFullnessPercentage += 16.66;
                    if (UserInfo.EmailConfirmed) ProfileFullnessPercentage += 16.66;
                    if (UserInfo.CreatedAt != null) ProfileFullnessPercentage += 16.66;
                    if (UserInfo.Link1 != null || UserInfo.Link2 != null) ProfileFullnessPercentage += 16.66;
                    if (UserInfo.UserName != UserInfo.PseudoName) ProfileFullnessPercentage += 16.66;

                    List<Country>? CountriesList;
                    if (UserInfo.CountryId == null) CountriesList = await _context.Countries.AsNoTracking().ToListAsync();
                    else
                    {
                        CountriesList = await _context.Countries.AsNoTracking().ToListAsync();
                        ProfileFullnessPercentage += 16.66;
                    }
                    int RoundedFullnessPercentage = (int)ProfileFullnessPercentage;

                    ViewBag.UserInfo = UserInfo;
                    ViewBag.LastPasswordChangeDays = DaysTillLastChange.Days;
                    ViewBag.PermissionToChangePassword = PermissionToChangePassword;
                    ViewBag.ProfileFullnessPercentage = Math.Round(ProfileFullnessPercentage, 1);
                    ViewBag.Int32FullnessPercentage = RoundedFullnessPercentage;
                    ViewBag.Countries = CountriesList;

                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Create", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> GetShortUserInfo(int Id)
        {
            GetUserInfo_ViewModel? Result = await _userRepository.GetUserByIdAsync(Id, true);
            if (Result != null) return Json(new { success = true, result = Result });
            else return Json(new { success = false });
        }

        public async Task<IActionResult> Verify(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                GetUserInfo_ViewModel? UserInfo = await _userRepository.GetUserByIdAsync(Id, true);
                ViewBag.UserInfo = UserInfo;

                return View();
            }
            return RedirectToAction("Create", "Account");
        }

        public async Task<IActionResult> Info(string? Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                IQueryable<Project?>? UserProjects;
                List<Project?>? UserProjectsResult = null;

                User? UserInfo = await _userManager.GetUserAsync(User);
                GetUserInfo_ViewModel? CurrentUserInfo = await _userRepository.GetUserBySearchnameAsync(Id);
                if (CurrentUserInfo != null)
                {
                    if (UserInfo != null)
                    {
                        UserProjects = _projectRepository.GetUsersAllProjects(CurrentUserInfo.Id, UserInfo.Id, false);
                        ViewBag.UserInfo = UserInfo;
                    }
                    else UserProjects = _projectRepository.GetUsersAllProjects(CurrentUserInfo.Id, 0, false);
                    if(UserProjects != null) UserProjectsResult = await UserProjects.ToListAsync();

                    if(CurrentUserInfo.Link1 != null) CurrentUserInfo.Link1 = _userRepository.LinkIconModifier(CurrentUserInfo?.Link1);
                    if(CurrentUserInfo?.Link2 != null) CurrentUserInfo.Link2 = _userRepository.LinkIconModifier(CurrentUserInfo?.Link2);

                    ViewBag.CurrentUserInfo = CurrentUserInfo;
                    ViewBag.UserProjects = UserProjectsResult;
                    ViewBag.ProjectsCount = UserProjectsResult?.Count;

                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyEmail_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _accountRepository.VerifyEmailAsync(Model);
                if (Result) return Json(new { success = true, alert = "Congrats! You've verified your email successfully. Now, you have secured your account and for that you've received some improved features of this application. More about by clicking the button" });

            }
            return Json(new { success = false, alert = "Something went wrong while trying to confirm your email. Please, try again later" });
        }       
    }
}
