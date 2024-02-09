using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IMention : IBase<Mention>
    {
        public Task<string?> SendMentionAsync(SendComment_ViewModel MentionModel);
        public Task<string?> EditMentionAsync(SendComment_ViewModel MentionModel);
        public IQueryable<Mention>? GetPostMentions(int Id, int SkipCount, int Count);
        public Task<int> RemoveMentionAsync(int Id, int PostId, int UserId);
        public Task<int> GetMentionsCountAsync(int Id);
    }
}
