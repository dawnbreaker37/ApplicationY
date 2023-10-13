using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ApplicationY.Repositories
{
    public class ProjectRepository : Base<Project>, IProject
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public ProjectRepository(Context context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
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
                    UserId = Model.UserId,
                    TargetPrice = Model.ProjectPrice,
                    PastTargetPrice = 0,
                    Views = 0,
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now
                };

                await _context.AddAsync(project);
                await _context.SaveChangesAsync();

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

                int Result = await _context.Projects.Where(p => p.Id == Model.Id && p.UserId == Model.UserId).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, Model.Name).SetProperty(p => p.Description, Model.Description).SetProperty(p => p.PriceChangeAnnotation, Model.TargetPriceChangeAnnotation).SetProperty(p => p.TextPart1, Part1).SetProperty(p => p.TextPart2, Part2).SetProperty(p => p.TextPart3, Part3).SetProperty(p => p.TargetPrice, Model.ProjectPrice).SetProperty(p => p.Link1, Model.Link1).SetProperty(p => p.Link2, Model.Link2).SetProperty(p => p.PastTargetPrice, PreviousPrice).SetProperty(p => p.YoutubeLink, Model.YoutubeLink).SetProperty(p => p.LastUpdatedAt, DateTime.Now));
                if (Result != 0) return Model.Name;
            }
            return null;
        }

        public async Task<int> RemoveAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Projects.Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<Project?> GetProjectAsync(int Id, bool GetAdditionalInfo)
        {
            Project? ProjectInfo = null;
            if (GetAdditionalInfo)
            {
                ProjectInfo = await _context.Projects.AsNoTracking().Select(p => new Project { Id = p.Id, Name = p.Name, Description = p.Description, TextPart1 = p.TextPart1, TextPart2 = p.TextPart2, TextPart3 = p.TextPart3, CreatedAt = p.CreatedAt, IsClosed = p.IsClosed, IsRemoved = p.IsRemoved, LastUpdatedAt = p.LastUpdatedAt, Link1 = p.Link1, Link2 = p.Link2, TargetPrice = p.TargetPrice, PastTargetPrice = p.PastTargetPrice, PriceChangeAnnotation = p.PriceChangeAnnotation, UserId = p.UserId, Views = p.Views, YoutubeLink = p.YoutubeLink }).FirstOrDefaultAsync(p => p.Id == Id && !p.IsRemoved);
                if(ProjectInfo != null)
                {
                    ProjectInfo.Views++;
                    _context.Update(ProjectInfo);
                    await _context.SaveChangesAsync();

                    return ProjectInfo;
                }
            }
            return await _context.Projects.AsNoTracking().Where(p => p.Id == Id && !p.IsRemoved).Select(p => new Project { Id = p.Id, Name = p.Name, Description = p.Description, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, IsClosed = p.IsClosed, TargetPrice = p.TargetPrice, PastTargetPrice = p.PastTargetPrice }).FirstOrDefaultAsync();
        }

        public IQueryable<Project?> GetUsersAllProjectsAsync(int UserId, bool GetAdditionalInfo)
        {
            if(UserId != 0)
            {
                if (!GetAdditionalInfo) return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, Description = p.Description, PastTargetPrice = p.PastTargetPrice, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, UserId = p.UserId }).OrderByDescending(p => p.CreatedAt);
                else return _context.Projects.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new Project { Id = p.Id, Name = p.Name, CreatedAt = p.CreatedAt, LastUpdatedAt = p.LastUpdatedAt, Description = p.Description, PastTargetPrice = p.PastTargetPrice, TargetPrice = p.TargetPrice, Views = p.Views, IsClosed = p.IsClosed, Link1 = p.Link1, Link2 = p.Link2, YoutubeLink = p.YoutubeLink, UserId = p.UserId });
            }
            return null;
        }

        public async Task<int> GetUsersLastProjectIdAsync(int UserId)
        {
            if(UserId != 0)
            {
                Project? LastProjectInfo = await _context.Projects.AsNoTracking().Select(u => new Project { Id = u.Id, UserId = u.UserId }).OrderBy(u => u.Id).FirstOrDefaultAsync(u => u.UserId == UserId);
                if (LastProjectInfo != null) return LastProjectInfo.Id;
            }
            return 0;
        }
    }
}
