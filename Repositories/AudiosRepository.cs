using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class AudiosRepository : Base<Audio>, IAudios
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AudiosRepository(Context context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<string>?> CreateAudioAsync(int ProjectId, IFormFileCollection Audios, string Description)
        {
            if(ProjectId != 0 && Audios != null)
            {
                List<string>? FileNames = new List<string>();
                int AudiosCount = Audios.Count;
                int CurrentAudiosCount = await _context.Audios.CountAsync(a => a.ProjectId == ProjectId);
                if(CurrentAudiosCount < 4)
                {
                    AudiosCount = 4 - CurrentAudiosCount;
                    for(int i = 0; i < AudiosCount; i++)
                    {
                        if (Audios[i] != null)
                        {
                            string? Extension = Path.GetExtension(Audios[i].FileName);
                            string? NewFileName = Guid.NewGuid().ToString("D").Substring(0, 8);
                            string? FullFileName = string.Concat(NewFileName, Extension);

                            using(FileStream fs= new FileStream(_webHostEnvironment.WebRootPath + "/ProjectAudios/" + FullFileName, FileMode.Create))
                            {
                                await Audios[i].CopyToAsync(fs);
                                Audio audio = new Audio
                                {
                                    Description = Description,
                                    IsRemoved = false,
                                    ProjectId = ProjectId,
                                    Name = FullFileName
                                };
                                FileNames = FileNames.Append(FullFileName).ToList();
                                await _context.AddAsync(audio);
                            }
                        }
                        await _context.SaveChangesAsync();
                        return FileNames;
                    }
                }
            }
            return null;
        }

        public async Task<int> GetAudiosCountAsync(int ProjectId)
        {
            return await _context.Audios.CountAsync(a => a.ProjectId == ProjectId && !a.IsRemoved);
        }

        public IQueryable<Audio>? GetProjectAudios(int ProjectId)
        {
            if (ProjectId != 0) return _context.Audios.AsNoTracking().Where(a => a.ProjectId == ProjectId && !a.IsRemoved);
            else return null;
        }

        public async Task<int> RemoveAudioAsync(int Id, int ProjectId)
        {
            if (Id != 0 && ProjectId != 0)
            {
                int Result = await _context.Audios.Where(a => a.ProjectId == ProjectId && a.Id == Id && !a.IsRemoved).ExecuteUpdateAsync(a => a.SetProperty(a => a.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }
    }
}
