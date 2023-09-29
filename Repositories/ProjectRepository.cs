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
                        Part1 = Model.TextPart.Substring(4000);
                    }
                    else if (Model.TextPart.Length > 4000 && Model.TextPart.Length <= 8000)
                    {
                        Part1 = Model.TextPart.Substring(4000);
                        Part2 = Model.TextPart.Substring(4000, 4000);
                    }
                    else if (Model.TextPart.Length > 8000)
                    {
                        Part1 = Model.TextPart.Substring(4000);
                        Part2 = Model.TextPart.Substring(4000, 4000);
                        Part3 = Model.TextPart.Substring(8000, 4000);
                    }
                    else
                    {
                        Part1 = Model.TextPart.Substring(4000);
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
                    PriceOfProject = Model.ProjectPrice
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

        public Task<Project?> GetUsersAllProjectsAsync(int UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetUsersLastProjectIdAsync(int UserId)
        {
            if(UserId != 0)
            {
                Project? LastProjectInfo = await _context.Projects.AsNoTracking().Select(u => new Project { Id = u.Id, UserId = u.UserId }).LastOrDefaultAsync(u => u.UserId == UserId);
                if(LastProjectInfo != null) return LastProjectInfo.Id;
            }
            return 0;
        }
    }
}
