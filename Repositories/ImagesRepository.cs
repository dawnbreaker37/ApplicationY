using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class ImagesRepository : Base<Image>, IImages
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImagesRepository(Context context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IQueryable<GetProjectImages_ViewModel>? GetAllProjectImages(int ProjectId)
        {
            if (ProjectId != 0) return _context.Images.AsNoTracking().Where(i => i.ProjectId == ProjectId && !i.IsRemoved).Select(i => new GetProjectImages_ViewModel { Id = i.Id, ProjectId = i.ProjectId, Name = i.Name, IsRemoved = i.IsRemoved });
            else return null;
        }

        public async Task<int> GetProjectImagesCountAsync(int ProjectId)
        {
            return await _context.Images.CountAsync(p => p.ProjectId == ProjectId && !p.IsRemoved);
        }

        public async Task<int> RemoveImageAsync(int Id, int ProjectId)
        {
            if(Id != 0 && ProjectId != 0)
            {
                int Result = await _context.Images.Where(i => i.Id == Id && i.ProjectId == ProjectId && !i.IsRemoved).ExecuteUpdateAsync(i => i.SetProperty(i => i.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }
    }
}
