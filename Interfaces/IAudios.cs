﻿using ApplicationY.Models;

namespace ApplicationY.Interfaces
{
    public interface IAudios
    {
        public Task<List<string>?> CreateAudioAsync(int ProjectId, IFormFileCollection Audios, List<string> Description);
        public Task<int> RemoveAudioAsync(int Id, int ProjectId);
        public IQueryable<Audio>? GetProjectAudios(int ProjectId);
        public Task<int> GetAudiosCountAsync(int ProjectId);
        public Task<string?> EditAudioTitleAsync(int Id, string Title);
    }
}
