using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
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

        public Task<Project?> GetProjectAsync(int Id)
        {
            throw new NotImplementedException();
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
