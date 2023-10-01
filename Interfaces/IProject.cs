using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IProject : IBase<Project>
    {
        public Task<string?> CreateAsync(CreateProject_ViewModel Model);
        public Task<Project?> GetProjectAsync(int Id);
        public IQueryable<Project?> GetUsersAllProjectsAsync(int UserId, bool GetAdditionalInfo);
        public Task<int> GetUsersLastProjectIdAsync(int UserId);
    }
}
