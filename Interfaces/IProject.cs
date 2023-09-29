using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IProject : IBase<Project>
    {
        public Task<string?> CreateAsync(CreateProject_ViewModel Model);
        public Task<Project?> GetProjectAsync(int Id);
        public Task<Project?> GetUsersAllProjectsAsync(int UserId);
        public Task<int> GetUsersLastProjectIdAsync(int UserId);
    }
}
