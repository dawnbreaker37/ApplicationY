using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IMessage
    {
        public Task<GetMessages_ViewModel?> GetMessageInfo(int Id, bool IsFromSender);
        public IQueryable<Message>? GetAllMessagesOfProject(int ProjectId);
        public IQueryable<GetMessages_ViewModel>? GetSentMessages(int UserId, bool GetRemovedToo);
        public IQueryable<GetMessages_ViewModel>? GetReceivedMessages(int UserId, bool GetRemovedToo);
        public Task<IQueryable<GetMessages_ViewModel>?> GetAdminsReceivedMessagesAsync(int AccessorId, bool GetRemovedToo, int SkippedCount, int LoadCount);
        public Task<int> GetReceivedMessagesCount(int AccessorId, int UserId, bool GetRemovedMessagesCountToo);
        public Task<bool> SendAsync(SendMessage_ViewModel Model);
        public Task<bool> SendCommentReplyAsync(SendReply_ViewModel Model);
        public Task<int> CheckAsync(int MessageId, int UserId);
        public Task<int> RemoveAsync(int Id, int UserId);
        public Task<int> RemoveForAdminsAsync(int Id);
        public IQueryable<GetCommentaries_ViewModel>? GetComments(int ProjectId, int SkipCount, int Count);
        public IQueryable<GetReplies_ViewModel>? GetReplies(int CommentId, int SkipCount, int LoadCount);
        public Task<int> SendCommentAsync(SendComment_ViewModel Model);
        public Task<string?> EditCommentAsync(SendComment_ViewModel Model);
        public Task<int> RemoveCommentAsync(int Id, int ProjectId, int UserId);
        public Task<int> GetProjectCommentsCountAsync(int ProjectId);
        public Task<int> GetCommentRepliesCountAsync(int CommentId);
        public Task<GetCommentaries_ViewModel?> GetLastCommentsInfoAsync(int ProjectId);
    }
}
