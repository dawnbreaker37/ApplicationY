using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class CategoryRepository : Base<Category>, ICategory
    {
        private readonly Context _context;
        public CategoryRepository(Context context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Category>? GetAll()
        {
            return _context.Categories.AsNoTracking();
        }

        public async Task<Category?>? GetCategoryAsync(int Id)
        {
            if (Id != 0) return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
            else return null;
        }

        public async Task<int> GetProjectsCountByThisCategory(int? Id)
        {
            if (Id != 0) return await _context.Projects.AsNoTracking().CountAsync(c => !c.IsRemoved && c.CategoryId == Id);
            else return 0;
        }
    }
}
