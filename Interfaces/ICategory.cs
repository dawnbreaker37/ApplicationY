using ApplicationY.Models;

namespace ApplicationY.Interfaces
{
    public interface ICategory
    {
        public IQueryable<Category>? GetAll();
        public Task<Category?>? GetCategoryAsync(int Id);
        public Task<int> GetProjectsCountByThisCategory(int? Id);
    }
}
