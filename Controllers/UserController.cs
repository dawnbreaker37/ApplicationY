﻿using ApplicationY.Data;
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
        private readonly ISubscribe _subscribeRepository;
        private readonly IPost _postRepository;
        private readonly IOthers _othersRepository;

        public UserController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, ISubscribe subscribeRepository, IOthers othersRepository, IPost postRepository, IAccount accountRepository, IUser userRepository, IProject projectRepository)
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
                    ViewBag.VerificationInfo = await _accountRepository.HasTheVerificationBeenSent(UserInfo.Id);
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
            GetUserInfo_ViewModel? Result = await _userRepository.GetUserByIdAsync(Id, true, false);
            if (Result != null) return Json(new { success = true, result = Result });
            else return Json(new { success = false });
        }

        public async Task<IActionResult> Verify(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                GetUserInfo_ViewModel? UserInfo = await _userRepository.GetUserByIdAsync(Id, true, false);
                ViewBag.UserInfo = UserInfo;

                return View();
            }
            return RedirectToAction("Create", "Account");
        }

        public async Task<IActionResult> Info(string? Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool IsSubscribed = false;
                string? TrueLink1 = null;
                string? TrueLink2 = null;

                //IQueryable<Project?>? UserProjects = null;
                IQueryable<GetPost_ViewModel>? Posts = null;
                //List<Project?>? UserProjectsResult = null;
                List<GetPost_ViewModel>? PostsResult = null;
                List<LikedPost>? LikedPosts = null;

                User? UserInfo = await _userManager.GetUserAsync(User);
                GetUserInfo_ViewModel? CurrentUserInfo = await _userRepository.GetUserBySearchnameAsync(Id);
                if (CurrentUserInfo != null)
                {
                    int SubscribersCount = await _subscribeRepository.GetSubscribersCount(CurrentUserInfo.Id);
                    if (UserInfo != null)
                    {
                        //UserProjects = _projectRepository.GetUsersAllProjects(CurrentUserInfo.Id, UserInfo.Id, false, false);
                        IsSubscribed = await _subscribeRepository.IsUserSubscribed(CurrentUserInfo.Id, UserInfo.Id);

                        ViewBag.UserInfo = UserInfo;
                    }
                    Posts = _postRepository.GetUserAllPosts(CurrentUserInfo.Id);
                    int ProjectsCount = await _projectRepository.GetProjectsCount(CurrentUserInfo.Id);

                    if (Posts != null) PostsResult = await Posts.ToListAsync();
                    if(PostsResult != null && UserInfo != null)
                    {
                        IQueryable<LikedPost>? LikedPosts_Preview = _postRepository.GetUsersLikedPosts(UserInfo.Id);
                        if(LikedPosts_Preview != null) LikedPosts = await LikedPosts_Preview.ToListAsync();
                        if(LikedPosts != null)
                        {
                            foreach (LikedPost Item in LikedPosts)
                            {
                                foreach(GetPost_ViewModel PostItem in PostsResult)
                                {
                                    if(Item.PostId == PostItem.Id)
                                    {
                                        PostItem.IsLiked = true;
                                    }
                                }
                            }
                        }
                    }

                    if (CurrentUserInfo.Link1 != null)
                    {
                        TrueLink1 = CurrentUserInfo.Link1;
                        CurrentUserInfo.Link1 = _userRepository.LinkIconModifier(CurrentUserInfo?.Link1);
                    }
                    if (CurrentUserInfo?.Link2 != null)
                    {
                        TrueLink2 = CurrentUserInfo.Link2;
                        CurrentUserInfo.Link2 = _userRepository.LinkIconModifier(CurrentUserInfo?.Link2);
                    }

                    ViewBag.CurrentUserInfo = CurrentUserInfo;
                    ViewBag.IsSubscribed = IsSubscribed;
                    ViewBag.Posts = PostsResult;
                    ViewBag.PostsCount = PostsResult == null ? 0 : PostsResult.Count;
                    ViewBag.SubscribersCount = SubscribersCount;
                    //ViewBag.UserProjects = UserProjectsResult;
                    ViewBag.ProjectsCount = ProjectsCount;
                    ViewBag.TrueLink1 = TrueLink1;
                    ViewBag.TrueLink2 = TrueLink2;

                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Create", "Account");
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
    }
}
