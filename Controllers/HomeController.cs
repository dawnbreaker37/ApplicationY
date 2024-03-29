﻿using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace ApplicationY.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly Context _context;
        private readonly IUser _user;
        private readonly ILogger<HomeController> _logger;
        private readonly IProject _projectRepository;
        private readonly ICategory _categoryRepository;
        private readonly ISubscribe _subscribeRepository;
        private readonly IMemoryCache _cache;
        private readonly IPost _postRepository;

        public HomeController(Context context, ILogger<HomeController> logger, IPost postRepository, IMemoryCache cache, ISubscribe subscribeRepository, ICategory categoryRepository, IUser user, IProject projectRepository, UserManager<User> userManager)
        {
            _context = context;
            _user = user;
            _cache = cache;
            _logger = logger;
            _userManager = userManager;
            _projectRepository = projectRepository;
            _categoryRepository = categoryRepository;
            _subscribeRepository = subscribeRepository;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<GetPost_ViewModel>? Posts = null;
            User? UserInfo = await _userManager.GetUserAsync(User);

            IQueryable<GetPost_ViewModel>? Posts_Preview = _postRepository.GetRandomPosts(30);
            if (Posts_Preview != null)
            {
                Posts = await Posts_Preview.ToListAsync();
            }

            if (UserInfo != null)
            {
                int LikedProjectsCount = 0;
                int SubscribtionsCount = 0;
                List<GetSubscribtions_ViewModel>? Subscribtions = null;
                List<GetLikedProjects_ViewModel>? LikedProjects = null;
                List<LikedPost>? LikedPosts = null;

                IQueryable<LikedPost>? LikedPosts_Preview = _postRepository.GetUsersLikedPosts(UserInfo.Id);
                IQueryable<GetSubscribtions_ViewModel>? UserSubscribtions_Preview = _subscribeRepository.GetSubscribtions(UserInfo.Id);
                IQueryable<GetLikedProjects_ViewModel>? LikedProjects_Preview = _projectRepository.GetLikedProjectsSimplified(UserInfo.Id);
                if(UserSubscribtions_Preview != null)
                {
                    Subscribtions = await UserSubscribtions_Preview.ToListAsync();
                    SubscribtionsCount = Subscribtions.Count;
                }
                if(LikedProjects_Preview != null)
                {
                    LikedProjects = await LikedProjects_Preview.ToListAsync();
                    LikedProjectsCount = LikedProjects.Count;
                }
                if(LikedPosts_Preview != null)
                {
                    LikedPosts = await LikedPosts_Preview.ToListAsync();
                }
                if(Posts != null && LikedPosts != null)
                {
                    foreach(GetPost_ViewModel Item in Posts)
                    {
                        foreach(LikedPost LikedPost in LikedPosts)
                        {
                            if(Item.Id == LikedPost.PostId)
                            {
                                Item.IsLiked = true;
                            }
                        }
                    }
                }

                UserInfo.Country = await _context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == UserInfo.CountryId);
                _cache.Set("user_" + UserInfo.Id, UserInfo, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12)));

                ViewBag.UserInfo = UserInfo;
                ViewBag.Subscribtions = Subscribtions;
                ViewBag.SubsCount = SubscribtionsCount;
                ViewBag.LikedProjectsCount = LikedProjectsCount;
                ViewBag.LikedProjects = LikedProjects;
            }
            else
            {
                ViewBag.UserInfo = null; ;
                ViewBag.SubsCount = 0;
                ViewBag.LikedProjectsCount = 0;
            }
            ViewBag.Posts = Posts;
            ViewBag.PostsCount = Posts != null ? Posts.Count : 0;

            return View();
        }

        public async Task<IActionResult> Search()
        {
            int Count = 0;
            int UsersCount = 0;
            int CategoriesCount = 0;
            int LastCategoryId = 0;
            User? UserInfo = null;
            List<GetProjects_ViewModel>? RandomProjects = null;
            List<GetUserInfo_ViewModel>? RandomUsers = null;
            List<Category>? Categories = null;
            if (User.Identity.IsAuthenticated)
            {
                UserInfo = await _userManager.GetUserAsync(User);

                IQueryable<Category>? Categories_Preview = _categoryRepository.GetAll();
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

                if(Categories_Preview != null)
                {
                    Categories = await Categories_Preview.ToListAsync();
                    LastCategoryId = Categories[Categories.Count - 1].Id;
                    CategoriesCount = Categories.Count;
                }
            }

            ViewBag.UserInfo = UserInfo;
            ViewBag.Projects = RandomProjects;
            ViewBag.ProjectsCount = Count;
            ViewBag.UsersCount = UsersCount;   
            ViewBag.Users = RandomUsers;
            ViewBag.Categories = Categories;
            ViewBag.CategoriesCount = CategoriesCount;
            ViewBag.LastCategoryId = LastCategoryId;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchEverywhere(string Keyword, int MinPrice, int MaxPrice, bool SearchOnlyProjects, bool SearchOnlyUsers, int CategoryId)
        {
            int ProjectsCount = 0;
            int UsersCount = 0;
            IQueryable<GetProjects_ViewModel>? ProjectsResult_Preview = null;
            IQueryable<GetUserInfo_ViewModel>? UsersResult_Preview = null;
            List<GetUserInfo_ViewModel>? UsersResult = null;
            List<GetProjects_ViewModel>? ProjectsResult = null;
            if (SearchOnlyProjects && !SearchOnlyUsers) ProjectsResult_Preview = _projectRepository.FindProjects(Keyword, MinPrice, MaxPrice, CategoryId, false, false);
            else if(!SearchOnlyProjects && SearchOnlyUsers) UsersResult_Preview = _user.FindUsers(Keyword, false);
            else
            {
                UsersResult_Preview = _user.FindUsers(Keyword, false);
                ProjectsResult_Preview = _projectRepository.FindProjects(Keyword, MinPrice, MaxPrice, CategoryId, false, false);
            }

            if(UsersResult_Preview != null)
            {
                UsersResult = await UsersResult_Preview.ToListAsync();
                UsersCount = UsersResult.Count;
            }
            if (ProjectsResult_Preview != null)
            {
                ProjectsResult = await ProjectsResult_Preview.ToListAsync();
                ProjectsCount = ProjectsResult.Count;
            }
            else
            {
                ProjectsResult = null;
                ProjectsCount = 0;
            }

            return Json(new { success = true, userResult = UsersResult, projectResult = ProjectsResult, usersCount = UsersCount, projectsCount = ProjectsCount });           
        }

        public async Task<IActionResult> About()
        {
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