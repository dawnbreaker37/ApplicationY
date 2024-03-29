﻿using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly ISubscribe _subscribeRepository;
        private readonly IPost _postRepository;
        private readonly IOthers _othersRepository;
        private readonly IMemoryCache _cache;

        public UserController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IMemoryCache cache, ISubscribe subscribeRepository, IOthers othersRepository, IPost postRepository, IAccount accountRepository, IUser userRepository, IProject projectRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _othersRepository = othersRepository;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _projectRepository = projectRepository;
            _subscribeRepository = subscribeRepository;
            _postRepository = postRepository;
            _cache = cache;
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
                    int UserRole = await _othersRepository.GetUserRoleAsync(UserInfo.Id);

                    ViewBag.UserInfo = UserInfo;
                    ViewBag.LastPasswordChangeDays = DaysTillLastChange.Days;
                    ViewBag.PermissionToChangePassword = PermissionToChangePassword;
                    ViewBag.ProfileFullnessPercentage = Math.Round(ProfileFullnessPercentage, 1);
                    ViewBag.Int32FullnessPercentage = RoundedFullnessPercentage;
                    ViewBag.VerificationInfo = await _accountRepository.HasTheVerificationBeenSent(UserInfo.Id);
                    ViewBag.Countries = CountriesList;
                    ViewBag.RoleId = UserRole;

                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Create", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> GetShortUserInfo(int Id, bool NeedsLargerInfo, bool IsForAdmins)
        {
            GetUserInfo_ViewModel? Result;
            GetUserRole_ViewModel? RoleInfo = null;
            if (IsForAdmins)
            {
                Result = await _userRepository.GetUserByIdAsync(Id, true);

                int UserRoleId = await _othersRepository.GetUserRoleAsync(Id);
                RoleInfo = await _othersRepository.GetUserFullRoleInfoAsync(Id, UserRoleId);
            }
            else 
            {
                if (NeedsLargerInfo) Result = await _userRepository.GetUserByIdWLargerInfoAsync(Id, false);
                else Result = await _userRepository.GetUserByIdAsync(Id, false);
            }

            if (Result != null) return Json(new { success = true, result = Result, roleInfo = RoleInfo });
            else return Json(new { success = false, alert = "We're sorry, but we haven't found any information about this user :(" });
        }

        [HttpGet]
        public async Task<IActionResult> GetSuperShortUserInfo(string Searchname)
        {
            if(!String.IsNullOrEmpty(Searchname))
            {
                GetUserInfo_ViewModel? Result = await _userRepository.GetSuperShortUserInfoBySearchnameAsync(Searchname);
                if (Result != null) return Json(new { success = true, result = Result });
                else return Json(new { success = false, alert = "We're sorry, but we haven't found any user by this searchname" });
            }
            return Json(new { success = false, alert = "An unexpected error has been occured. Please, try again later" });
        }

        public async Task<IActionResult> Verify(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                GetUserInfo_ViewModel? UserInfo = await _userRepository.GetUserByIdWLargerInfoAsync(Id, false);
                ViewBag.UserInfo = UserInfo;

                return View();
            }
            return RedirectToAction("Create", "Account");
        }

        public async Task<IActionResult> Info(string? Id)
        {
            bool IsSubscribed = false;
            IQueryable<GetPost_ViewModel>? Posts = null;
            List<GetPost_ViewModel>? PostsResult = null;
            List<LikedPost>? LikedPosts = null;
            GetUserInfo_ViewModel? CurrentUserInfo = await _userRepository.GetUserBySearchnameAsync(Id);
            User? UserInfo = null;

            if (CurrentUserInfo != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    UserInfo = await _userManager.GetUserAsync(User);
                }
                int SubscribersCount = await _subscribeRepository.GetSubscribersCount(CurrentUserInfo.Id);
                if (UserInfo != null)
                {
                    IsSubscribed = await _subscribeRepository.IsUserSubscribed(CurrentUserInfo.Id, UserInfo.Id);
                }
                Posts = _postRepository.GetUserAllPosts(CurrentUserInfo.Id, true);
                int ProjectsCount = await _projectRepository.GetProjectsCount(CurrentUserInfo.Id);
                if (Posts != null) PostsResult = await Posts.ToListAsync();
                if (PostsResult != null && UserInfo != null)
                {
                    IQueryable<LikedPost>? LikedPosts_Preview = _postRepository.GetUsersLikedPosts(UserInfo.Id);
                    if (LikedPosts_Preview != null) LikedPosts = await LikedPosts_Preview.ToListAsync();
                    if (LikedPosts != null)
                    {
                        foreach (LikedPost Item in LikedPosts)
                        {
                            foreach (GetPost_ViewModel PostItem in PostsResult)
                            {
                                if (Item.PostId == PostItem.Id)
                                {
                                    PostItem.IsLiked = true;
                                }
                            }
                        }
                    }
                }
                ViewBag.UserInfo = UserInfo;
                ViewBag.CurrentUserInfo = CurrentUserInfo;
                ViewBag.IsSubscribed = IsSubscribed;
                ViewBag.Posts = PostsResult;
                ViewBag.PostsCount = PostsResult == null ? 0 : PostsResult.Count;
                ViewBag.SubscribersCount = SubscribersCount;
                ViewBag.ProjectsCount = ProjectsCount;

                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> PurgeTheUser(int UserId, int SenderId, string Description)
        {
            int Result = await _othersRepository.SendPurgeAsync(0, SenderId, UserId, Description);
            if (Result != 0) return Json(new { success = true, alert = "Purge request for this user has been successfully sent. Please, wait. The answer for your request will be sent as soon as it's possible. You'll receive it as a simple notification which will appear in your notifications list. Thank you!" });
            else return Json(new { success = false, alert = "You've already sent a purge request for this user. Please, wait for your answer, or check it in your notifications list. Thank you!" });
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

        [HttpGet]
        public async Task<IActionResult> SearchForUsers(string Keyword)
        {
            IQueryable<GetUserInfo_ViewModel>? Users_Preview = _userRepository.FindUsers(Keyword, true);
            if(Users_Preview != null)
            {
                List<GetUserInfo_ViewModel>? Users = await Users_Preview.ToListAsync();
                return Json(new { success = true, result = Users, count = Users.Count });
            }
            return Json(new { success = false, alert = "No found users by this keyword. Please, try another" });
        }
    }
}
