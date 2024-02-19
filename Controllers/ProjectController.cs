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
        private readonly IMessage _messageRepository;
        private readonly ICategory _categoryRepository;
        private readonly IImages _imagesRepository;
        private readonly IAudios _audiosRepository;
        private readonly IOthers _othersRepository;
        private readonly IAccount _accountRepository;
        private readonly IPost _postRepository;
        private readonly Context _context;

        public ProjectController(UserManager<User> userManager, IPost postRepository, IAccount accountRepository, IOthers othersRepository, IImages imagesRepository, IAudios audiosRepository, ICategory categoryRepository, IProject projectRepository, IMessage messageRepository, Context context)
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _context = context;
            _messageRepository = messageRepository;
            _categoryRepository = categoryRepository;
            _imagesRepository = imagesRepository;
            _audiosRepository = audiosRepository;
            _othersRepository = othersRepository;
            _accountRepository = accountRepository;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                int LastCategoryId = 0;
                int Count = 0;

                List<Category>? Categories = null;
                IQueryable<Category>? Categories_Preview = _categoryRepository.GetAll();
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (Categories_Preview != null)
                {
                    Categories = await Categories_Preview.ToListAsync();
                    LastCategoryId = Categories.Select(c => c.Id).LastOrDefault();
                    Count = Categories.Count;
                }

                ViewBag.UserInfo = UserInfo;
                ViewBag.Categories = Categories;
                ViewBag.LastCategoryId = LastCategoryId;
                ViewBag.Count = Count;

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
                //User? UserInfo = await _accountRepository.GetCurrentUserFromCacheAsync(Int32_Id);
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                {
                    int ProjectsCount = 0;
                    int RemovedProjectsCount = 0;
                    int PostsCount = 0;
                    //await Response.Cookies.
                    IQueryable<Project?>? UserAllProjects_Preview = _projectRepository.GetUsersAllProjectsWInfo(UserInfo.Id, UserInfo.Id, true);
                    IQueryable<Project?>? UserAllRemovedProjects_Preview = _projectRepository.GetUsersAllRemovedProjects(UserInfo.Id);
                    //IQueryable<GetPost_ViewModel>? GetUserAllPosts_Preview = _postRepository.GetUserAllPosts(UserInfo.Id, false);

                    List<Project?>? RemovedProjects = UserAllRemovedProjects_Preview != null ? await UserAllRemovedProjects_Preview.ToListAsync() : null;
                    List<Project?>? Projects = UserAllProjects_Preview != null ? await UserAllProjects_Preview.ToListAsync() : null;
                    //List<GetPost_ViewModel>? AllPosts = GetUserAllPosts_Preview != null ? await GetUserAllPosts_Preview.ToListAsync() : null;
                    if(Projects != null)
                    {
                        ProjectsCount = Projects.Count;
                        RemovedProjectsCount = RemovedProjects != null ? RemovedProjects.Count : 0;
                        PostsCount = await _postRepository.GetUsersAllPostsCountAsync(UserInfo.Id);
                    }

                    ViewBag.UserInfo = UserInfo;
                    ViewBag.Projects = Projects;
                    ViewBag.RemovedProjects = RemovedProjects;
                    ViewBag.RemovedProjectsCount = RemovedProjectsCount;
                    ViewBag.Count = ProjectsCount;
                    ViewBag.PostsCount = PostsCount;

                    return View();
                }
            }
            else return RedirectToAction("Create", "Account");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> PurgeTheProject(int Id, int SenderId, string Description)
        {
            int Result = await _othersRepository.SendPurgeAsync(Id, SenderId, 0, Description);
            if (Result != 0) return Json(new { success = true, alert = "Purge request has been successfully sent. Please, wait. Your asnwer will in 1-2 day(s) by notification which will appear in your notifications list. Thank you!", projectId = Id });
            else return Json(new { success = false, alert = "You've already sent a purge request for this project. Please, wait for your answer or check your notifications list. May be the is waiting for you!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts(int Id)
        {
            IQueryable<GetPost_ViewModel>? Posts_Preview = _postRepository.GetUserAllPosts(Id, false);
            if (Posts_Preview != null)
            {
                List<GetPost_ViewModel>? Posts = await Posts_Preview.ToListAsync();
                if (Posts != null) return Json(new { success = true, result = Posts });
            }
            return Json(new { success = false, alert = "You've got no posts. Create at least one post to edit, lock/unlock or remove it from this page" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects(ProjectFilters_ViewModel Model)
        {
            User? UserInfo = await _userManager.GetUserAsync(User);
            if (UserInfo != null)
            {
                Model.UserId = UserInfo.Id;
                if (ModelState.IsValid)
                {
                    IQueryable<Project?>? Projects_Review = _projectRepository.GetUserAllProjectsByFilters(Model);
                    if (Projects_Review != null)
                    {
                        List<Project?>? Projects = await Projects_Review.ToListAsync();
                        int Count = Projects.Count;

                        ViewBag.UserInfo = UserInfo;
                        ViewBag.Projects = Projects;
                        ViewBag.Count = Count;

                        return View("All");
                    }
                }
            }
            return RedirectToAction("All", "Project");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                {
                    int Count = 0;
                    int ImgsCount = 0;
                    int AudiosCount = 0;
                    int LastCategoryId = 0;
                    double TotalDays = 0;
                    List<Category>? Categories = null;
                    List<GetProjectImages_ViewModel>? Images = null;
                    List<Audio>? Audios = null;
                    IQueryable<Category>? Categories_Preview = null;
                    IQueryable<GetProjectImages_ViewModel>? Images_Preview = null;
                    IQueryable<Audio>? Audios_Preview = null;

                    Project? ProjectInfo = await _projectRepository.GetProjectToEditAsync(Id, UserInfo.Id);
                    if (ProjectInfo != null && ProjectInfo.UserId == UserInfo.Id)
                    {
                        string? MainFullText = null;
                        StringBuilder? FullText = new StringBuilder(ProjectInfo.TextPart1);
                        if (!String.IsNullOrEmpty(ProjectInfo.TextPart2)) FullText = FullText?.Append(ProjectInfo.TextPart2);
                        if (!String.IsNullOrEmpty(ProjectInfo.TextPart3)) FullText = FullText?.Append(ProjectInfo.TextPart3);

                        double PercentageOfTargetPrice = Math.Round((double)(ProjectInfo.TargetPrice / 10000000 * 100), 1);
                        if (FullText != null) MainFullText = FullText?.ToString();

                        Images_Preview = _imagesRepository.GetAllProjectImages(Id, 0);
                        Audios_Preview = _audiosRepository.GetProjectAudios(Id);
                        Categories_Preview = _categoryRepository.GetAll();
                        if (Categories_Preview != null)
                        {
                            Categories = await Categories_Preview.ToListAsync();
                            LastCategoryId = Categories.Select(c => c.Id).LastOrDefault();
                            Count = Categories.Count;
                        }
                        if (Audios_Preview != null)
                        {
                            Audios = await Audios_Preview.ToListAsync();
                            AudiosCount = Audios.Count;
                        }

                        if (ProjectInfo.Deadline != null)
                        {
                            TimeSpan StartDay = DateTime.Now.Subtract((DateTime)ProjectInfo.Deadline);
                            TotalDays = Math.Round(StartDay.TotalDays, 0) * -1;
                        }

                        if (Images_Preview != null)
                        {
                            Images = await Images_Preview.ToListAsync();
                            ImgsCount = Images.Count;
                        }

                        ViewBag.UserInfo = UserInfo;
                        ViewBag.Audios = Audios;
                        ViewBag.AudiosCount = AudiosCount;
                        ViewBag.ProjectInfo = ProjectInfo;
                        ViewBag.FullText = MainFullText;
                        ViewBag.FullTextLength = MainFullText?.Length;
                        ViewBag.PercentageOfTargetPrice = PercentageOfTargetPrice;
                        ViewBag.Categories = Categories;
                        ViewBag.LastCategoryId = LastCategoryId;
                        ViewBag.Count = Count;
                        ViewBag.Images = Images;
                        ViewBag.ImgsCount = ImgsCount;
                        ViewBag.TotalDays = TotalDays;

                        return View();
                    }
                    else return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> EditProjectImages(int Id, IFormFileCollection Images)
        {
            if (ModelState.IsValid)
            {
                List<string?>? Result = await _projectRepository.EditImagesAsync(Id, Images);
                if (Result != null) return Json(new { success = true, alert = "Selected images has been successfully uploaded", result = Result });
            }
            return Json(new { success = false, alert = "An error occured while trying to upload images. Please, try again" });
        }

        [HttpPost]
        public async Task<IActionResult> EditAudioTitle(int Id, string Title)
        {
            string? Result = await _audiosRepository.EditAudioTitleAsync(Id, Title);
            if (Result != null) return Json(new { success = true, alert = "Selected audio title has been successfully updated" });
            else return Json(new { success = false, alert = "Unable to edit selected audio's title. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> SetAsMainImg(int Id, int ProjectId)
        {
            int Result = await _imagesRepository.SetAsMainAsync(Id, ProjectId);
            if (Result != 0) return Json(new { success = true, alert = "Main image has been successfully changed", image = Result });
            else return Json(new { success = false, alert = "Unable to change main image. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> EditProjectAudios(int Id, IFormFileCollection Audios)
        {
            if (ModelState.IsValid)
            {
                List<string>? Result = await _audiosRepository.CreateAudioAsync(Id, Audios, null);
                if (Result != null) return Json(new { success = true, alert = "Audio files has been successfully added", result = Result });
                else return Json(new { success = false, alert = "An error occured while trying to edit your project audio files. Please, try again later" });
            }
            else return Json(new { success = false, alert = "Something went wrong so we can't add these audios" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAudio(int Id, int ProjectId)
        {
            int Result = await _audiosRepository.RemoveAudioAsync(Id, ProjectId);
            if (Result != 0) return Json(new { success = true, alert = "Selected audio has been successfully removed from your project", result = Result });
            else return Json(new { success = false, alert = "Unable to remove this project. Please, try again later" });
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
            return Json(new { success = false, alert = "Something from the entered datas are wrong. Please, check them all and then try again" });
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdate(int Id, string Title, string Description)
        {
            bool Result = await _projectRepository.AddReleaseNoteAsync(Id, Title, Description);
            if (Result) return Json(new { success = true, alert = "Release note has been successfully created. It'll appear at your project main page" });
            else return Json(new { success = false, alert = "Unable to create release note. Please, check all entered datas and then try again" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUpdates(int Id)
        {
            IQueryable<GetReleaseNotes_ViewModel>? ReleaseNotes_Preview = _projectRepository.GetReleaseNotes(Id);
            if (ReleaseNotes_Preview != null)
            {
                List<GetReleaseNotes_ViewModel>? ReleaseNotes = await ReleaseNotes_Preview.ToListAsync();
                return Json(new { success = true, result = ReleaseNotes, count = ReleaseNotes.Count });
            }
            else return Json(new { success = false, alert = "There's no actual release notes for this project" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(int Id, int ProjectId)
        {
            int Result = await _imagesRepository.RemoveImageAsync(Id, ProjectId);
            if (Result != 0) return Json(new { success = true, alert = "Image has been successfully removed", result = Result });
            else return Json(new { success = false, alert = "Unable to delete selected image" });
        }

        public async Task<IActionResult> Liked()
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                {
                    int Count = 0;
                    List<GetLikedProjects_ViewModel>? LikedProjectsResult = null;
                    IQueryable<GetLikedProjects_ViewModel>? LikedProjectsResult_Preview = _projectRepository.GetLikedProjects(UserInfo.Id);
                    if (LikedProjectsResult_Preview != null)
                    {
                        LikedProjectsResult = await LikedProjectsResult_Preview.ToListAsync();
                        Count = LikedProjectsResult.Count;
                    }

                    ViewBag.UserInfo = UserInfo;
                    ViewBag.LikedProjects = LikedProjectsResult;
                    ViewBag.Count = Count;

                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id, int UserId)
        {
            int Result = await _projectRepository.RemoveAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, Id = Result, alert = "Selected project has successfully removed" });
            else return Json(new { success = false, alert = "Unable to remove selected project at this moment. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> LockTheProject(int Id, int UserId)
        {
            int Result = await _projectRepository.CloseProjectAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, alert = "Selected project has been successfully locked. Now, it's invisible for all users for up to 7 days", id = Id });
            else return Json(new { success = false, alert = "An error occured while trying to lock selected project" });
        }

        [HttpPost]
        public async Task<IActionResult> UnlockTheProject(int Id, int UserId)
        {
            int Result = await _projectRepository.UnlockProjectAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, alert = "The selected project is now visible again for everyone.", id = Id });
            else return Json(new { success = false, alert = "An error occured while trying to unlock the selected project" });
        }

        [HttpPost]
        public async Task<IActionResult> Like(int ProjectId, int UserId)
        {
            int Result = await _projectRepository.LikeTheProjectAsync(ProjectId, UserId);
            if (Result != 0) return Json(new { success = true, projectId = ProjectId });
            else return Json(new { success = false, alert = "An unexpected error occured. May be, you've already liked this project" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromLiked(int ProjectId, int UserId)
        {
            int Result = await _projectRepository.RemoveFromLikesAsync(ProjectId, UserId, false);
            if (Result != 0) return Json(new { success = true, projectId = ProjectId });
            else return Json(new { success = false, alert = "An error occured while trying to remove the project from your liked projects list. May be, you've already removed it from your liked list" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllLiked(int UserId)
        {
            int Result = await _projectRepository.RemoveFromLikesAsync(0, UserId, true);
            if (Result != 0) return Json(new { success = true, projectId = Result });
            else return Json(new { success = false, alert = "An error occured while trying to remove your all liked projects" });
        }

        [HttpPost]
        public async Task<IActionResult> Pin(int Id, int UserId)
        {
            int Result = await _projectRepository.PinAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, result = Result, alert = "Project has been successfully pinned" });
            else return Json(new { success = false, alert = "Project has recently been pinned" });
        }

        [HttpPost]
        public async Task<IActionResult> Unpin(int Id, int UserId)
        {
            int Result = await _projectRepository.UnpinAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, result = Result, alert = "Project has been successfully unpinned" });
            else return Json(new { success = false, alert = "Project has recently been unpinned" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFullProject(int Id, int SenderId, bool GetUsername, bool GetFiles)
        {
            Project? ProjectInfo = await _projectRepository.GetProjectAsync(Id, SenderId, GetUsername);
            if (ProjectInfo != null)
            {
                int ImagesCount = 0;
                List<Audio>? Audios = null;
                GetProjectImages_ViewModel? MainImg = null;
                List<GetProjectImages_ViewModel>? Images = null;
                bool IsLiked = false;
                int LikesCount = await _projectRepository.ProjectLikesCount(Id);

                if (GetFiles)
                {
                    IQueryable<Audio>? Audios_Preview = _audiosRepository.GetProjectAudios(Id);
                    IQueryable<GetProjectImages_ViewModel>? Images_Preview = _imagesRepository.GetAllProjectImages(Id, ProjectInfo.MainPhotoId);

                    if (Audios_Preview != null) Audios = await Audios_Preview.ToListAsync();
                    if (Images_Preview != null) Images = await Images_Preview.ToListAsync();
                    if (ProjectInfo.MainPhotoId != 0)
                    {
                        MainImg = await _imagesRepository.GetSingleImgAsync(ProjectInfo.MainPhotoId, Id);
                        if (MainImg != null) ImagesCount = Images == null ? 0 : Images.Count + 1;
                        else ImagesCount = Images == null ? 0 : Images.Count;
                    }
                }

                if (SenderId != 0) IsLiked = await _projectRepository.HasProjectBeenAlreadyLiked(Id, SenderId);
                await _projectRepository.IncreaseProjectViews(Id, ProjectInfo.Views, 1);

                return Json(new { success = true, getUsername = GetUsername, likesCount = LikesCount, imgsCount = ImagesCount, result = ProjectInfo, isLiked = IsLiked, audios = Audios, mainImg = MainImg, audiosCount = Audios != null ? Audios.Count : 0, images = Images });
            }
            else return Json(new { success = false, alert = "Unable to have a look on this project at this moment. Please, try again later" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFullProject_ForAdmins(int Id)
        {
            Project? ProjectInfo = await _projectRepository.GetAllProjectAsync(Id, false, false);
            if(ProjectInfo != null)
            {
                return Json(new { success = true, result = ProjectInfo });
            }
            return Json(new { success = false, alert = "An error has been occured. Please, try again later" });
        }

        public async Task<IActionResult> Info(int Id)
        {
            if(Id != 0)
            {
                Project? ProjectInfo = await _projectRepository.GetProjectAsync(Id, 0, true);
                if(ProjectInfo != null)
                {
                    if (!ProjectInfo.IsRemoved)
                    {
                        List<GetProjectImages_ViewModel>? Images = null;
                        List<Audio>? Audios = null;

                        GetCommentaries_ViewModel? LastSentComment = null;
                        string? YoutubeVideoLink = null;
                        string? AdditionalYoutubeLink = null;
                        bool HasAlreadyBeenLiked = false;
                        double PreviousPricePercentage = 0;
                        double TotalDays = 0;
                        int AudiosCount = 0;
                        int ImagesCount = 0;
                        int CommentsCount = 0;

                        int ReleaseNotesCount = await _projectRepository.ReleaseNotesCountAsync(Id);
                        int LikesCount = await _projectRepository.ProjectLikesCount(Id);
                        int FullProjectsCount = await _projectRepository.GetProjectsCount(0);
                        double CategoryStatistics = await _categoryRepository.GetProjectsCountByThisCategory(ProjectInfo.CategoryId);
                        double CategoryPercentage = Math.Round(CategoryStatistics / FullProjectsCount * 100, 1);
                        IQueryable<GetProjectImages_ViewModel>? Images_Preview = _imagesRepository.GetAllProjectImages(Id, ProjectInfo.MainPhotoId);
                        IQueryable<Audio>? AudiosPreview = _audiosRepository.GetProjectAudios(Id);
                        GetProjectImages_ViewModel? MainImgInfo = await _imagesRepository.GetSingleImgAsync(ProjectInfo.MainPhotoId, Id);

                        if (!ProjectInfo.AreCommentsDisabled)
                        {
                            CommentsCount = await _messageRepository.GetProjectCommentsCountAsync(Id);
                            if (CommentsCount != 0) LastSentComment = await _messageRepository.GetLastCommentsInfoAsync(Id);
                        }

                        StringBuilder sb = new StringBuilder();
                        sb.Append(ProjectInfo.TextPart1);
                        if (ProjectInfo.TextPart2 != null) sb.Append(ProjectInfo.TextPart2);
                        if (ProjectInfo.TextPart3 != null) sb.Append(ProjectInfo.TextPart3);

                        if (ProjectInfo.Deadline != null)
                        {
                            TimeSpan StartDay = DateTime.Now.Subtract((DateTime)ProjectInfo.Deadline);
                            TotalDays = Math.Round(StartDay.TotalDays, 0) * -1;
                        }

                        if (ProjectInfo.PastTargetPrice != 0)
                        {
                            PreviousPricePercentage = (double)ProjectInfo.TargetPrice / ProjectInfo.PastTargetPrice * 100;
                            if (PreviousPricePercentage < 100) PreviousPricePercentage = Math.Round(100 - PreviousPricePercentage, 1) * -1;
                            else PreviousPricePercentage = Math.Round(PreviousPricePercentage - 100, 1);
                        }

                        if (ProjectInfo.YoutubeLink != null)
                        {
                            YoutubeVideoLink = "https://www.youtube.com/embed/" + ProjectInfo.YoutubeLink;
                            AdditionalYoutubeLink = "https://www.youtube.com/watch?v=" + ProjectInfo.YoutubeLink;
                        }

                        if (Images_Preview != null)
                        {
                            Images = await Images_Preview.ToListAsync();
                            if (MainImgInfo != null) Images = Images.Prepend(MainImgInfo).ToList();
                            ImagesCount = Images.Count;
                        }
                        if (AudiosPreview != null)
                        {
                            Audios = await AudiosPreview.ToListAsync();
                            AudiosCount = Audios.Count;
                        }

                        if (User.Identity.IsAuthenticated)
                        {
                            User? UserInfo = await _userManager.GetUserAsync(User);

                            if (UserInfo != null)
                            {
                                HasAlreadyBeenLiked = await _projectRepository.HasProjectBeenAlreadyLiked(Id, UserInfo.Id);
                                ViewBag.UserInfo = UserInfo;
                            }
                            else ViewBag.UserInfo = null;
                        }
                        int LinkedProjectsCount = await _postRepository.GetAssociatedPostsCountAsync(Id);
                        await _projectRepository.IncreaseProjectViews(Id, ProjectInfo.Views, 1);

                        ViewBag.ProjectInfo = ProjectInfo;
                        ViewBag.TotalDays = TotalDays;
                        ViewBag.Images = Images;
                        ViewBag.Audios = Audios;
                        ViewBag.ImagesCount = ImagesCount;
                        ViewBag.LinkedProjectsCount = LinkedProjectsCount;
                        ViewBag.MainImgInfo = MainImgInfo;
                        ViewBag.AudiosCount = AudiosCount;
                        ViewBag.ReleaseNotesCount = ReleaseNotesCount;
                        ViewBag.TargetPriceChangePerc = PreviousPricePercentage;
                        ViewBag.FullText = sb;
                        ViewBag.LikesCount = LikesCount;
                        ViewBag.CommentsCount = CommentsCount;
                        ViewBag.LastSentCommentInfo = LastSentComment;
                        ViewBag.IsLiked = HasAlreadyBeenLiked;
                        ViewBag.YTLink = YoutubeVideoLink;
                        ViewBag.ProjectsByThisCategory = CategoryStatistics;
                        ViewBag.CategoryPercentage = CategoryPercentage;
                        ViewBag.AdditionalYTLink = AdditionalYoutubeLink;

                        return View();
                    }
                    else
                    {
                        return RedirectToAction("RemovedProject", "Project", Id);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemovedProject(int Id)
        {
            ViewBag.ProjectId = Id;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FindProjects(string Keyword)
        {
            IQueryable<GetProjects_ViewModel>? Project_Preview = _projectRepository.FindProjects(Keyword, 0, 0, 0, true, true);
            if (Project_Preview != null)
            {
                List<GetProjects_ViewModel>? ProjectsResult = await Project_Preview.ToListAsync();
                int ProjectsCount = ProjectsResult.Count;

                if (ProjectsResult != null) return Json(new { success = true, result = ProjectsResult, count = ProjectsCount });
                else return Json(new { success = false, alert = "No projects found by this keyword. Please, try another one" });
            }
            else return Json(new { success = false, alert = "Something went wrong. Please, try again later" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProjects(int Id, bool ForAdmins)
        {
            IQueryable<Project?>? Projects_Preview = _projectRepository.GetUsersAllProjects(Id, 0, ForAdmins);
            if(Projects_Preview != null)
            {
                List<Project?>? Projects = await Projects_Preview.ToListAsync();
                if (Projects != null) return Json(new { success = true, result = Projects, count = Projects.Count });
            }
            return Json(new { success = false, alert = "We haven't found any project from this user so, there's nothing to load" });
        }

        [HttpPost]
        public async Task<IActionResult> Disable(int Id, int DisablerId, string Description)
        {
            int Result = await _projectRepository.DisableProjectAsync(Id, DisablerId, Description);
            if (Result != 0) return Json(new { success = true, result = Result, alert = "Selected project has been successfully disabled" });
            else return Json(new { success = false, alert = "We can't disable this project right now. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> Enable(int Id, int DisablerId)
        {
            int Result = await _projectRepository.EnableProjectAsync(Id, DisablerId);
            if (Result != 0) return Json(new { success = true, result = Result, alert = "Selected project has been successfully enabled" });
            else return Json(new { success = false, alert = "We can't enable this project right now. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(CreatePost_ViewModel Model)
        {
            if(ModelState.IsValid)
            {
                int Result = await _postRepository.EditPostAsync(Model);
                if (Result != 0) return Json(new { success = true, alert = "Selected post has been successfully updated", result = Result });
            }
            return Json(new { success = false, alert = "An unexpected error has been occured. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> LockThePost(int Id, int UserId)
        {
            int Result = await _postRepository.LockThePostAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, alert = "Selected post has been locked. Now it has became private", result = Result });
            else return Json(new { success = false, alert = "An unexpected error has been occured. Please, try to lock the post again later" });
        }

        [HttpPost]
        public async Task<IActionResult> UnlockThePost(int Id, int UserId)
        {
            int Result = await _postRepository.UnlockThePostAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, alert = "Selected post has been unlocked", result = Result });
            else return Json(new { success = false, alert = "An unexpected error has been occured. Please, try to unlock the post again later" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveThePost(int Id, int UserId)
        {
            int Result = await _postRepository.RemovePostAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, result = Result });
            else return Json(new { success = false, alert = "Unable to remove the selected post. Please, check all datas and then try again" });
        }
    }
}
