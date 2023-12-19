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

        public IQueryable<GetProjectImages_ViewModel>? GetAllProjectImages(int ProjectId, int MainImgId)
        {
            if (ProjectId != 0)
            {
                if (MainImgId == 0) return _context.Images.AsNoTracking().Where(i => i.ProjectId == ProjectId && !i.IsRemoved).Select(i => new GetProjectImages_ViewModel { Id = i.Id, ProjectId = i.ProjectId, Name = i.Name, IsRemoved = i.IsRemoved });
                else return _context.Images.AsNoTracking().Where(i => i.ProjectId == ProjectId && !i.IsRemoved && i.Id != MainImgId).Select(i => new GetProjectImages_ViewModel { Id = i.Id, ProjectId = i.ProjectId, Name = i.Name, IsRemoved = i.IsRemoved });
            }
            else return null;
        }

        public async Task<int> GetProjectImagesCountAsync(int ProjectId)
        {
            return await _context.Images.AsNoTracking().CountAsync(p => p.ProjectId == ProjectId && !p.IsRemoved);
        }

        public async Task<GetProjectImages_ViewModel?> GetSingleImgAsync(int Id, int ProjectId)
        {
            if (ProjectId != 0)
            {
                if(Id != 0) return await _context.Images.AsNoTracking().Select(i => new GetProjectImages_ViewModel { Id = i.Id, IsRemoved = i.IsRemoved, ProjectId = i.ProjectId, Name = i.Name }).FirstOrDefaultAsync(i => i.Id == Id && i.ProjectId == ProjectId && !i.IsRemoved);
                else return await _context.Images.AsNoTracking().Select(i => new GetProjectImages_ViewModel { Id = i.Id, IsRemoved = i.IsRemoved, ProjectId = i.ProjectId, Name = i.Name }).FirstOrDefaultAsync(i => i.ProjectId == ProjectId && !i.IsRemoved);
            }
            else return null;
        }

        public async Task<int> RemoveImageAsync(int Id, int ProjectId)
        {
            if(Id != 0 && ProjectId != 0)
            {
                bool IsThisTheMainImg = await _context.Projects.AnyAsync(p => p.Id == ProjectId && p.MainPhotoId == Id);
                if (IsThisTheMainImg)
                {
                    int OtherImgId = await _context.Images.AsNoTracking().Where(i => i.ProjectId == ProjectId && !i.IsRemoved && i.Id != Id).Select(i => i.Id).FirstOrDefaultAsync();
                    await _context.Projects.Where(p => p.Id == ProjectId).ExecuteUpdateAsync(p => p.SetProperty(p => p.MainPhotoId, OtherImgId));
                }
                int Result = await _context.Images.AsNoTracking().Where(i => i.Id == Id && i.ProjectId == ProjectId && !i.IsRemoved).ExecuteUpdateAsync(i => i.SetProperty(i => i.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> SetAsMainAsync(int Id, int ProjectId)
        {
            if(Id != 0 && ProjectId != 0)
            {
                int SelectedImgId = await _context.Images.AsNoTracking().Where(i => i.Id == Id && !i.IsRemoved && i.ProjectId == ProjectId).Select(i => i.Id).FirstOrDefaultAsync();
                if(SelectedImgId != 0)
                {
                    int Result = await _context.Projects.AsNoTracking().Where(p => p.Id == ProjectId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.MainPhotoId, SelectedImgId));
                    if (Result != 0) return SelectedImgId;
                }
            }
            return 0;
        }
    }
}
