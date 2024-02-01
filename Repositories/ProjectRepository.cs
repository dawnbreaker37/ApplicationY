using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class ProjectRepository : Base<Project>, IProject
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImages _imagesRepository;

        public ProjectRepository(Context context, UserManager<User> userManager, IImages imagesRepository, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _imagesRepository = imagesRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string?> CreateAsync(CreateProject_ViewModel Model)
        {
            if (!String.IsNullOrEmpty(Model.Name) && !String.IsNullOrEmpty(Model.Description))
            {
                string? Part1 = null;
                string? Part2 = null;
                string? Part3 = null;
                if (!String.IsNullOrEmpty(Model.TextPart))
                {
                    if (Model.TextPart.Length <= 4000)
                    {
                        Part1 = Model.TextPart;
                    }
                    else if (Model.TextPart.Length > 4000 && Model.TextPart.Length <= 8000)
                    {
                        Part1 = Model.TextPart.Substring(0, 4000);
                        Part2 = Model.TextPart.Substring(4000, Model.TextPart.Length - 4000);
                    }
                    else if (Model.TextPart.Length > 8000)
                    {
                        Part1 = Model.TextPart.Substring(0, 4000);
                        Part2 = Model.TextPart.Substring(4000, 4000);
                        Part3 = Model.TextPart.Substring(8000, Model.TextPart.Length - 8000);
                    }
                    else
                    {
                        Part1 = Model.TextPart;
                    }
                }

                if (Model.Duration > 0 && Model.Duration <= 1200)
                {
                    DateTime? PredictedDate = DateTime.Now.AddDays(Model.Duration);
                    Model.Deadline = PredictedDate;
                }
                else Model.Deadline = null;

                if (Model.YoutubeLink != null)
                {
                    if (Model.YoutubeLink.Contains("?"))
                    {
                        Model.YoutubeLink = Model.YoutubeLink.Substring(Model.YoutubeLink.LastIndexOf("=") + 1);
                    }
                    else if (Model.YoutubeLink.Contains("embed"))
                    {
                        Model.YoutubeLink = Model.YoutubeLink.Substring(Model.YoutubeLink.LastIndexOf("/") + 1);
                    }
                }

                Project project = new Project
                {
                    Name = Model.Name,
                    Description = Model.Description,
                    Link1 = Model.Link1,
                    Link2 = Model.Link2,
                    YoutubeLink = Model.YoutubeLink,
                    TextPart1 = Part1,
                    TextPart2 = Part2,
                    TextPart3 = Part3,
                    Deadline = Model.Deadline,
                    UserId = Model.UserId,
                    TargetPrice = Model.ProjectPrice,
                    CategoryId = Model.CategoryId,
                    PastTargetPrice = 0,
                    Views = 0,
                    IsBudget = Model.IsBudget,
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now
                };

                await _context.AddAsync(project);
                await _context.SaveChangesAsync();
                if(Model.ImagesFiles != null) await EditImagesAsync(project.Id, Model.ImagesFiles);

                return Model.Name;
            }
            return null;
        }

        public async Task<string?> EditAsync(CreateProject_ViewModel Model)
        {
            if(!String.IsNullOrEmpty(Model.Name) && (!String.IsNullOrEmpty(Model.Description) && (!String.IsNullOrEmpty(Model.TextPart))))
            {
                int PreviousPrice = 0;
                string? Part1;
                string? Part2 = null;
                string? Part3 = null;
                if (Model.TextPart.Length <= 4000)
                {
                    Part1 = Model.TextPart;
                }
                else if (Model.TextPart.Length > 4000 && Model.TextPart.Length <= 8000)
                {
                    Part1 = Model.TextPart.Substring(0, 4000);
                    Part2 = Model.TextPart.Substring(4000, Model.TextPart.Length - 4000);
                }
                else if (Model.TextPart.Length > 8000)
                {
                    Part1 = Model.TextPart.Substring(0, 4000);
                    Part2 = Model.TextPart.Substring(4000, 4000);
                    Part3 = Model.TextPart.Substring(8000, Model.TextPart.Length - 8000);
                }
                else
                {
                    Part1 = Model.TextPart;
                }
                if (Model.CurrentPrice != Model.ProjectPrice) PreviousPrice = Model.CurrentPrice;

                if (Model.Duration > 0 && Model.Duration <= 1200)
                {
                    DateTime? PredictedDate = DateTime.Now.AddDays(Model.Duration);
                    Model.Deadline = PredictedDate;
                }
                else Model.Deadline = null;

                if (Model.YoutubeLink != null)
                {
                    if (Model.YoutubeLink.Contains("?"))
                    {
                        Model.YoutubeLink = Model.YoutubeLink.Substring(Model.YoutubeLink.LastIndexOf("=") + 1);
                    }
                    else if (Model.YoutubeLink.Contains("embed"))
                    {
                        Model.YoutubeLink = Model.YoutubeLink.Substring(Model.YoutubeLink.LastIndexOf("/") + 1);
                    }
                }

                int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == Model.Id && p.UserId == Model.UserId).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, Model.Name).SetProperty(p => p.Description, Model.Description).SetProperty(p => p.IsBudget, Model.IsBudget).SetProperty(p => p.DonationRules, Model.DonationRules).SetProperty(p => p.PriceChangeAnnotation, Model.TargetPriceChangeAnnotation).SetProperty(p => p.CategoryId, Model.CategoryId).SetProperty(p => p.Deadline, Model.Deadline).SetProperty(p => p.TextPart1, Part1).SetProperty(p => p.TextPart2, Part2).SetProperty(p => p.TextPart3, Part3).SetProperty(p => p.TargetPrice, Model.ProjectPrice).SetProperty(p => p.Link1, Model.Link1).SetProperty(p => p.Link2, Model.Link2).SetProperty(p => p.PastTargetPrice, PreviousPrice).SetProperty(p => p.YoutubeLink, Model.YoutubeLink).SetProperty(p => p.LastUpdatedAt, DateTime.Now));
                if (Result != 0) return Model.Name;
            }
            return null;
        }

        public async Task<int> RemoveAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<Project?> GetProjectAsync(int Id, int UserId, bool GetUsername, bool GetAdditionalInfo)
        {
            Project? ProjectInfo = null;
            if (GetAdditionalInfo)
            {
                if (GetUsername) ProjectInfo = await _context.Projects.AsNoTracking().Select(p => new Project { Id = p.Id, MainPhotoId = p.MainPhotoId, CategoryName = p.Category == null ? null : p.Category.Name, DonationRules = p.DonationRules, CategoryDescription = p.Category == null ? null : p.Category.Description, IsBudget = p.IsBudget, Deadline = p.Deadline, Name = p.Name, Description = p.Description, TextPart1 = p.TextPart1, CategoryId = p.CategoryId, TextPart2 = p.TextPart2, TextPart3 = p.TextPart3, CreatedAt = p.CreatedAt, IsClosed = p.IsClosed, IsRemoved = p.IsRemoved, LastUpdatedAt = p.LastUpdatedAt, Link1 = p.Link1, Link2 = p.Link2, TargetPrice = p.TargetPrice, UserName = p.User!.PseudoName, PastTargetPrice = p.PastTargetPrice, PriceChangeAnnotation = p.PriceChangeAnnotation, UserId = p.UserId, Views = p.Views, YoutubeLink = p.YoutubeLink }).FirstOrDefaultAsync(p => p.Id == Id && !p.IsRemoved && (p.UserId == UserId || !p.IsClosed));
                else ProjectInfo = await _context.Projects.AsNoTracking().Select(p => new Project { Id = p.Id, MainPhotoId = p.MainPhotoId, CategoryName = p.Category == null ? null : p.Category.Name, IsBudget = p.IsBudget, DonationRules = p.DonationRules, Deadline = p.Deadline, CategoryDescription = p.Category == null ? null : p.Category.Description, Name = p.Name, Description = p.Description, TextPart1 = p.TextPart1, CategoryId = p.CategoryId, TextPart2 = p.TextPart2, TextPart3 = p.TextPart3, CreatedAt = p.CreatedAt, IsClosed = p.IsClosed, IsRemoved = p.IsRemoved, LastUpdatedAt = p.LastUpdatedAt, Link1 = p.Link1, Link2 = p.Link2, TargetPrice = p.TargetPrice, PastTargetPrice = p.PastTargetPrice, PriceChangeAnnotation = p.PriceChangeAnnotation, UserId = p.UserId, Views = p.Views, YoutubeLink = p.YoutubeLink }).FirstOrDefaultAsync(p => p.Id == Id && !p.IsRemoved && (p.UserId == UserId && (!p.IsClosed || p.IsClosed) || !p.IsClosed));

                if (ProjectInfo != null)
                {
                    ProjectInfo.Views++;
                    _context.Update(ProjectInfo);
                    await _context.SaveChangesAsync();

                    return ProjectInfo;
                }
                else return null;
            }
            else return await _context.Projects.AsNoTracking().Where(p => p.Id == Id && !p.IsRemoved && (p.UserId == UserId || !p.IsClosed)).Select(p => new Project { Id = p.Id, Name = p.Name, Description = p.Description, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, IsClosed = p.IsClosed, IsBudget = p.IsBudget, TargetPrice = p.TargetPrice, PastTargetPrice = p.PastTargetPrice }).FirstOrDefaultAsync();
        }

        public async Task<Project?> GetAllProjectAsync(int Id, bool GetUsername, bool GetAdditionalInfo)
        {
            if (GetAdditionalInfo)
            {
                if(GetUsername) return await _context.Projects.AsNoTracking().Select(p => new Project { Id = p.Id, Description = p.Description, CreatedAt = p.CreatedAt, TextPart1 = p.TextPart1, TextPart2 = p.TextPart2, TextPart3 = p.TextPart3, MainPhotoId = p.MainPhotoId, Link1 = p.Link1, Link2 = p.Link2, Name = p.Name, Deadline = p.Deadline, IsBudget = p.IsBudget, Duration = p.Duration, PastTargetPrice = p.PastTargetPrice, PriceChangeAnnotation = p.PriceChangeAnnotation, YoutubeLink = p.YoutubeLink, CategoryName = p.Category != null ? p.Category.Name : null, LastUpdatedAt = p.LastUpdatedAt, UserName = p.User!.PseudoName, Views = p.Views, IsRemoved = p.IsRemoved, IsClosed = p.IsClosed, TargetPrice = p.TargetPrice, UserId = p.UserId }).FirstOrDefaultAsync(p => p.Id == Id);
                else return await _context.Projects.AsNoTracking().Select(p => new Project { Id = p.Id, Description = p.Description, CreatedAt = p.CreatedAt, TextPart1 = p.TextPart1, TextPart2 = p.TextPart2, TextPart3 = p.TextPart3, MainPhotoId = p.MainPhotoId, Link1 = p.Link1, Link2 = p.Link2, Name = p.Name, Deadline = p.Deadline, IsBudget = p.IsBudget, Duration = p.Duration, PastTargetPrice = p.PastTargetPrice, PriceChangeAnnotation = p.PriceChangeAnnotation, YoutubeLink = p.YoutubeLink, CategoryName = p.Category != null ? p.Category.Name : null, LastUpdatedAt = p.LastUpdatedAt, Views = p.Views, IsRemoved = p.IsRemoved, IsClosed = p.IsClosed, TargetPrice = p.TargetPrice }).FirstOrDefaultAsync(p => p.Id == Id);
            }
            else return await _context.Projects.AsNoTracking().Select(p => new Project { Id = p.Id, Description = p.Description, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, Views = p.Views, IsRemoved = p.IsRemoved, IsClosed = p.IsClosed, TargetPrice = p.TargetPrice, UserId = p.UserId }).FirstOrDefaultAsync(p => p.Id == Id);
        }

        public IQueryable<Project?>? GetUsersAllProjects(int UserId, int SenderId, bool GetAdditionalInfo, bool GetCompressed, bool ForAdmins)
        {
            if(UserId != 0)
            {
                if (!GetAdditionalInfo)
                {
                    if(!GetCompressed)
                    {
                        if (UserId == SenderId) return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, Description = p.Description, IsPinned = p.IsPinned, PastTargetPrice = p.PastTargetPrice, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, UserId = p.UserId }).OrderByDescending(p => p.IsPinned).ThenByDescending(p => p.CreatedAt);
                        else return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved && !p.IsClosed).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, Description = p.Description, IsPinned = p.IsPinned, PastTargetPrice = p.PastTargetPrice, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, UserId = p.UserId }).OrderByDescending(p => p.IsPinned).ThenByDescending(p => p.CreatedAt);
                    }
                    else
                    {
                        if (!ForAdmins)
                        {
                            if (UserId == SenderId) return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, UserId = p.UserId }).OrderByDescending(p => p.CreatedAt);
                            else return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved && !p.IsClosed).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, UserId = p.UserId }).OrderByDescending(p => p.CreatedAt);
                        }
                        else
                        {
                            return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, Views = p.Views, IsRemoved = p.IsRemoved, IsClosed = p.IsClosed, LastUpdatedAt = p.LastUpdatedAt, TargetPrice = p.TargetPrice }).OrderByDescending(p => p.LastUpdatedAt);
                        }
                    }
                }
                else
                {
                    if(UserId == SenderId) return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, IsPinned = p.IsPinned, LastUpdatedAt = p.LastUpdatedAt, Description = p.Description, PastTargetPrice = p.PastTargetPrice, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, Link1 = p.Link1, Link2 = p.Link2, YoutubeLink = p.YoutubeLink, UserId = p.UserId }).OrderByDescending(p => p.IsPinned).ThenByDescending(p => p.CreatedAt);
                    else return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved && !p.IsClosed).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, IsPinned = p.IsPinned, LastUpdatedAt = p.LastUpdatedAt, Description = p.Description, PastTargetPrice = p.PastTargetPrice, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, Link1 = p.Link1, Link2 = p.Link2, YoutubeLink = p.YoutubeLink, UserId = p.UserId }).OrderByDescending(p => p.IsPinned).ThenByDescending(p => p.CreatedAt);
                }
            }
            return null;
        }

        public async Task<int> GetUsersLastProjectIdAsync(int UserId)
        {
            if(UserId != 0)
            {
                Project? LastProjectInfo = await _context.Projects.AsNoTracking().OrderBy(u => u.CreatedAt).Select(u => new Project { Id = u.Id, UserId = u.UserId, IsRemoved = u.IsRemoved }).FirstOrDefaultAsync(u => u.UserId == UserId && u.IsRemoved);
                if (LastProjectInfo != null) return LastProjectInfo.Id;
            }
            return 0;
        }

        public async Task<int> CloseProjectAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == Id && !p.IsRemoved && !p.IsClosed).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsClosed, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> UnlockProjectAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved && p.IsClosed).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsClosed, false));
                if(Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> LikeTheProjectAsync(int ProjectId, int UserId)
        {
            if(ProjectId != 0 && UserId != 0)
            {
                bool IsThereAnythingLikeThat = await _context.Likes.AnyAsync(l => l.ProjectId == ProjectId && l.UserId == UserId && !l.IsRemoved);
                if (IsThereAnythingLikeThat) return 0;
                else
                {
                    Like like = new Like
                    {
                        ProjectId = ProjectId,
                        UserId = UserId,
                        IsRemoved = false
                    };
                    await _context.AddAsync(like);
                    await _context.SaveChangesAsync();

                    return ProjectId;
                }
            }
            return 0;
        }

        public async Task<int> RemoveFromLikesAsync(int ProjectId, int UserId, bool RemoveAll)
        {
            if (!RemoveAll) {
                if (ProjectId != 0 && UserId != 0)
                {
                    int Result = await _context.Likes.AsNoTracking().Where(l => l.ProjectId == ProjectId && l.UserId == UserId && !l.IsRemoved).ExecuteUpdateAsync(l => l.SetProperty(l => l.IsRemoved, true));
                    if (Result != 0) return ProjectId;
                }
            }
            else
            {
                if(UserId != 0)
                {
                    int Result = await _context.Likes.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                    if (Result != 0) return -256;
                }
            }
            return 0;
        }

        public async Task<bool> HasProjectBeenAlreadyLiked(int ProjectId, int UserId)
        {
            if(ProjectId != 0 && UserId != 0)
            {
                bool Result = await _context.Likes.AnyAsync(l => l.ProjectId == ProjectId && l.UserId == UserId && !l.IsRemoved);
                return Result;
            }
            else return false;
        }

        public async Task<int> ProjectLikesCount(int ProjectId)
        {
            if (ProjectId != 0) return await _context.Likes.AsNoTracking().CountAsync(l => l.ProjectId == ProjectId && !l.IsRemoved);
            else return 0;
        }

        public IQueryable<Project?>? GetUserAllProjectsByFilters(ProjectFilters_ViewModel Model)
        {
            if (Model.UserId != 0)
            {
                if (Model.GetAdditionalInfo) return _context.Projects.AsNoTracking().Where(p => (p.UserId == Model.UserId) && (!p.IsRemoved) && (!p.IsClosed) && ((Model.Keyword == null) || (p.Name != null && p.Name.ToLower().Contains(Model.Keyword.ToLower())) || (p.Description != null && p.Description.ToLower().Contains(Model.Keyword.ToLower()))) && ((Model.MinTargetPrice == 0 && Model.MaxTargetPrice == 0) || (Model.MaxTargetPrice != 0 && p.TargetPrice <= Model.MaxTargetPrice && p.TargetPrice >= Model.MinTargetPrice || Model.MaxTargetPrice == 0 && p.TargetPrice <= 10000000 && p.TargetPrice >= Model.MinTargetPrice))).Select(p => new Project { Id = p.Id, Name = p.Name, IsPinned = p.IsPinned, Description = p.Description, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, IsClosed = p.IsClosed, PastTargetPrice = p.TargetPrice, PriceChangeAnnotation = p.PriceChangeAnnotation, Link1 = p.Link1, Link2 = p.Link2, TextPart1 = p.TextPart1, TextPart2 = p.TextPart2, TextPart3 = p.TextPart3, TargetPrice = p.TargetPrice, UserId = p.UserId, Views = p.Views, YoutubeLink = p.YoutubeLink }).OrderByDescending(p => p.IsPinned).ThenByDescending(p => p.LastUpdatedAt);
                else return _context.Projects.AsNoTracking().Where(p => (p.UserId == Model.UserId) && (!p.IsRemoved) && (!p.IsClosed) && ((Model.Keyword == null || p.Name != null && p.Name.ToLower().Contains(Model.Keyword.ToLower()) || p.Description != null && p.Description.ToLower().Contains(Model.Keyword.ToLower())) && (Model.MinTargetPrice == 0 || (Model.MaxTargetPrice != 0 && p.TargetPrice <= Model.MaxTargetPrice && p.TargetPrice >= Model.MinTargetPrice || Model.MaxTargetPrice == 0 && p.TargetPrice <= 10000000 && p.TargetPrice >= Model.MinTargetPrice)))).Select(p => new Project { Id = p.Id, Name = p.Name, Description = p.Description, CreatedAt = p.CreatedAt, IsPinned = p.IsPinned, LastUpdatedAt = p.LastUpdatedAt, IsClosed = p.IsClosed, PastTargetPrice = p.TargetPrice, TargetPrice = p.TargetPrice, UserId = p.UserId, Views = p.Views }).OrderByDescending(p => p.IsPinned).ThenByDescending(p => p.LastUpdatedAt);
            }
            else return null;
        }

        public IQueryable<GetProjects_ViewModel>? GetRandomProjects(int Count)
        {
            if (Count != 0) return _context.Projects.AsNoTracking().Where(p => p.Views >= 150 && !p.IsClosed && !p.IsRemoved && (p.LastUpdatedAt.Year == DateTime.Now.Year && p.LastUpdatedAt.DayOfYear >= DateTime.Now.DayOfYear - 124) || (p.LastUpdatedAt.Year == DateTime.Now.Year - 1 && p.LastUpdatedAt.DayOfYear >= 350)).Select(p=>new GetProjects_ViewModel { Name = p.Name, Id = p.Id, CreatedAt = p.CreatedAt, LastUpdatedAt =  p.LastUpdatedAt, Views = p.Views, TargetPrice = p.TargetPrice, UserName = p.User!.PseudoName, UserId = p.UserId }).Take(Count).OrderByDescending(p => p.Views);
            else return _context.Projects.AsNoTracking().Where(p => p.Views >= 150 && !p.IsClosed && !p.IsRemoved && (p.LastUpdatedAt.Year == DateTime.Now.Year && p.LastUpdatedAt.DayOfYear <= DateTime.Now.DayOfYear - 14) || (p.LastUpdatedAt.Year == DateTime.Now.Year - 1 && p.LastUpdatedAt.DayOfYear >= 350)).Select(p => new GetProjects_ViewModel { Name = p.Name, Id = p.Id, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, Views = p.Views, TargetPrice = p.TargetPrice, UserName = p.User!.PseudoName, UserId = p.UserId  }).Take(10).OrderByDescending(p => p.Views);
        }

        public IQueryable<GetProjects_ViewModel>? FindProjects(string Keyword, int MinPrice, int MaxPrice, int CategoryId, bool LoadSimplified, bool GetRemovedToo)
        {
            if (!LoadSimplified)
            {
                return _context.Projects.AsNoTracking().Where(p => (!p.IsRemoved && !p.IsClosed) && (CategoryId == 0 || p.CategoryId == CategoryId) && (Keyword == null || (p.Name!.ToLower().Contains(Keyword.ToLower())) || (p.Description!.ToLower().Contains(Keyword.ToLower())) || (p.TextPart1!.ToLower().Contains(Keyword.ToLower())) || (p.TextPart2 != null && p.TextPart2.ToLower().Contains(Keyword.ToLower())) || (p.TextPart3 != null && p.TextPart3.ToLower().Contains(Keyword.ToLower()))) && (MinPrice == 0 && MaxPrice == 0 || p.TargetPrice >= MinPrice && p.TargetPrice <= MaxPrice)).Select(p => new GetProjects_ViewModel { Id = p.Id, LastUpdatedAt = p.LastUpdatedAt, CreatedAt = p.CreatedAt, Name = p.Name, TargetPrice = p.TargetPrice, PastTargetPrice = p.PastTargetPrice, UserName = p.User!.PseudoName, Views = p.Views, UserId = p.UserId, IsClosed = p.IsClosed, IsRemoved = p.IsRemoved }).OrderByDescending(p => p.Views).ThenByDescending(p => p.LastUpdatedAt);
            }
            else
            {
                if (GetRemovedToo) return _context.Projects.AsNoTracking().Where(p => Keyword == null || p.Name!.ToLower().Contains(Keyword.ToLower()) || p.Description!.ToLower().Contains(Keyword.ToLower()) || p.TextPart1 != null && p.TextPart1.ToLower().Contains(Keyword.ToLower()) || p.TextPart2 != null && p.TextPart2.ToLower().Contains(Keyword.ToLower()) || p.TextPart3 != null && p.TextPart3.ToLower().Contains(Keyword.ToLower())).Select(p => new GetProjects_ViewModel { Id = p.Id, Name = p.Name, UserName = p.User!.PseudoName, UserId = p.UserId, IsRemoved = p.IsRemoved });
                else return _context.Projects.AsNoTracking().Where(p => !p.IsRemoved && !p.IsClosed && Keyword == null || p.Name!.ToLower().Contains(Keyword.ToLower()) || p.Description!.ToLower().Contains(Keyword.ToLower()) || p.TextPart1 != null && p.TextPart1.ToLower().Contains(Keyword.ToLower()) || p.TextPart2 != null && p.TextPart2.ToLower().Contains(Keyword.ToLower()) || p.TextPart3 != null && p.TextPart3.ToLower().Contains(Keyword.ToLower())).Select(p => new GetProjects_ViewModel { Id = p.Id, Name = p.Name, UserName = p.User!.PseudoName, UserId = p.UserId });
            }
        }

        public async Task<int> GetProjectsCount(int UserId)
        {
            if (UserId == 0) return await _context.Projects.AsNoTracking().CountAsync(p => !p.IsRemoved);
            else return await _context.Projects.AsNoTracking().CountAsync(p => !p.IsRemoved && p.UserId == UserId && !p.IsClosed);
        }

        public async Task<bool> AddReleaseNoteAsync(int ProjectId, string Title, string Description)
        {
            if(ProjectId != 0 && !String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Description))
            {
                bool IsProjectReadyForReleaseNote = await _context.Projects.AnyAsync(p => !p.IsRemoved && p.Id == ProjectId);
                if (IsProjectReadyForReleaseNote)
                {
                    Update update = new Update
                    {
                        Title = Title,
                        Description = Description,
                        UpdatedAt = DateTime.Now,
                        IsRemoved = false,
                        ProjectId = ProjectId
                    };
                    await _context.Updates.AddAsync(update);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            return false;
        }

        public async Task<int> ReleaseNotesCountAsync(int ProjectId)
        {
            return await _context.Updates.AsNoTracking().CountAsync(p => !p.IsRemoved && p.ProjectId == ProjectId);
        }

        public IQueryable<GetReleaseNotes_ViewModel>? GetReleaseNotes(int ProjectId)
        {
            if (ProjectId != 0) return _context.Updates.AsNoTracking().Where(p => !p.IsRemoved && p.ProjectId == ProjectId).Select(p => new GetReleaseNotes_ViewModel { Id = p.Id, Description = p.Description, Title = p.Title, ReleaseDate = p.UpdatedAt }).OrderByDescending(p => p.ReleaseDate);
            else return null;
        }

        public IQueryable<GetLikedProjects_ViewModel>? GetLikedProjects(int UserId)
        {
            if (UserId != 0) return _context.Likes.AsNoTracking().Where(l => l.UserId == UserId && !l.IsRemoved).Select(l => new GetLikedProjects_ViewModel { Id = l.Id, CreatorName = l.Project!.User!.PseudoName, Name = l.Project.Name, Views = l.Project.Views, CreatorSearchName = l.Project.User.SearchName, CreateAtDate = l.Project.CreatedAt, ProjectId = l.ProjectId });
            else return null;
        }

        public async Task<int> PinAsync(int Id, int UserId)
        {
            if (Id != 0 || UserId != 0) 
            {
                int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved && !p.IsPinned).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsPinned, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> UnpinAsync(int Id, int UserId)
        {
            if (Id != 0 || UserId != 0)
            {
                int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved && p.IsPinned).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsPinned, false));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public IQueryable<GetLikedProjects_ViewModel>? GetLikedProjectsSimplified(int UserId)
        {
            if (UserId != 0) return _context.Likes.AsNoTracking().Where(l => l.UserId == UserId && !l.IsRemoved).Select(l => new GetLikedProjects_ViewModel { Id = l.Id, Name = l.Project!.Name, CreatorName = l.Project.User!.PseudoName, ProjectId = l.ProjectId });
            else return null;
        }

        public async Task<List<string?>?> EditImagesAsync(int Id, IFormFileCollection Images)
        {
            if(Id != 0 && Images != null)
            {
                int CurrentImgsCount = await _imagesRepository.GetProjectImagesCountAsync(Id);
                if(CurrentImgsCount < 6)
                {
                    int FirstImgId = 0;
                    int Count = Images.Count;
                    if(CurrentImgsCount != 0) Count = 6 - CurrentImgsCount;

                    if (Count > 0 && Count <= 6)
                    {
                        //int MainImgId = await _context.Projects.AsNoTracking().Where(p => p.Id == Id).Select(p => p.MainPhotoId).FirstOrDefaultAsync();
                        List<string?> FileNames = new List<string?>();
                        for(int i = 0; i < Count; i++)
                        {
                            string? Extension = Path.GetExtension(Images[i].FileName);
                            string? FullName = Guid.NewGuid().ToString("D").Substring(0, 12);
                            FullName = string.Concat(FullName, Extension);
                            using (FileStream fs = new FileStream(_webHostEnvironment.WebRootPath + "/ProjectPhotos/" + FullName, FileMode.Create))
                            {
                                await Images[i].CopyToAsync(fs);
                                Image Image = new Image
                                {
                                    Name = FullName,
                                    ProjectId = Id
                                };
                                await _context.AddAsync(Image);
                                await _context.SaveChangesAsync();
                                if (i == 0 && CurrentImgsCount == 0)
                                {
                                    FirstImgId = Image.Id;
                                    await _context.Projects.Where(p => p.Id == Id && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.MainPhotoId, FirstImgId));
                                }
                            }
                            FileNames = FileNames.Append(FullName).ToList();
                        }
                        return FileNames;
                    }
                }
            }
            return null;
        }

        public async Task<int> DisableProjectAsync(int Id, int DisablerId, string Description)
        {
            bool IsTheDisablerAnAdmin = await _context.UserRoles.AnyAsync(x => x.UserId == DisablerId);
            if(IsTheDisablerAnAdmin && (!String.IsNullOrEmpty(Description) && Description.Length >= 40 && Description.Length < 320))
            {
                int Result = await _context.Projects.Where(p => p.Id == Id).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                if(Result != 0)
                {
                    DisabledProject disabledProject = new DisabledProject
                    {
                        Description = Description,
                        DisabledAt = DateTime.Now,
                        ProjectId = Id
                    };
                    await _context.AddAsync(disabledProject);
                    await _context.SaveChangesAsync();

                    return Id;
                }
            }
            return 0;
        }

        public async Task<int> EnableProjectAsync(int Id, int DisablerId)
        {
            bool IsTheDisablerAnAdmin = await _context.UserRoles.AnyAsync(x => x.UserId == DisablerId);
            if (IsTheDisablerAnAdmin)
            {
                int Result = await _context.Projects.Where(p => p.Id == Id).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, false));
                if (Result != 0)
                {
                    await _context.DisabledProjects.Where(p => p.ProjectId == Id).ExecuteDeleteAsync();
                    return Id;
                }
            }
            return 0;
        }
    }
}
