using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IMessage
    {
        public Task<GetMessages_ViewModel?> GetMessageInfo(int Id);
        public IQueryable<Message>? GetAllMessagesOfProject(int ProjectId);
        public IQueryable<GetMessages_ViewModel>? GetAllMyMessages(int UserId, bool GetSentMessages);
        public Task<bool> SendAsync(SendMessage_ViewModel Model);
        public Task<bool> SendCommentReplyAsync(SendReply_ViewModel Model);
        public Task<int> CheckAsync(int MessageId, int UserId);
        public Task<int> RemoveAsync(int Id, int UserId);
        public IQueryable<GetCommentaries_ViewModel>? GetComments(int ProjectId, int SkipCount, int Count);
        public IQueryable<GetReplies_ViewModel>? GetReplies(int CommentId, int SkipCount, int LoadCount);
        public Task<int> SendCommentAsync(SendComment_ViewModel Model);
        public Task<int> GetProjectCommentsCountAsync(int ProjectId);
        public Task<int> GetCommentRepliesCountAsync(int CommentId);
        public Task<GetCommentaries_ViewModel?> GetLastCommentsInfoAsync(int ProjectId);
    }
}
