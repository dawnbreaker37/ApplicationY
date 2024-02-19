using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class MessagesController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IUser _userRepository;
        private readonly IMessage _messageRepository;

        public MessagesController(Context context, UserManager<User> userManager, IUser userRepository, IMessage messageRepository)
        {
            _context = context;
            _userManager = userManager;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendMessage_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _messageRepository.SendAsync(Model);
                if (Result) return Json(new { success = true, alert = "Message has been sent" });
                else return Json(new { success = false, alert = "Message hasn't been sent. Please, check all datas and try again" });
            }
            else return Json(new { success = false, alert = "An error occured while trying to send the message. Please, be sure that the message text is correct and then try to send it again" });
        }

        [HttpPost]
        public async Task<IActionResult> SendToAdmins(SendMessage_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _messageRepository.SendAsync(Model);
                if (Result) return Json(new { success = true, alert = "Your message has been successfully sent. We'll reply to your account or email if haven't got account yet" });
                else return Json(new { success = false, alert = "Unable to send message. Please, try again later, may be the system is overloaded now" });
            }
            else return Json(new { success = false, alert = "Something went wrong. Please, check all datas and then try again" });
        }

        [HttpPost]
        public async Task<IActionResult> SendReply(SendReply_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _messageRepository.SendCommentReplyAsync(Model);
                if (Result) return Json(new { success = true, text = Model.Text, commentId = Model.CommentId });                
            }
            return Json(new { success = false, alert = "An error occured while trying to send your reply. Please, check all datas and then try again" });
        }

        [HttpGet]
        public async Task<IActionResult> GetReplies(int Id, int SkipCount, int LoadCount)
        {
            IQueryable<GetReplies_ViewModel>? Replies_Preview = _messageRepository.GetReplies(Id, SkipCount, LoadCount);
            if (Replies_Preview != null)
            {
                List<GetReplies_ViewModel>? Replies = await Replies_Preview.ToListAsync();
                int Count = await _messageRepository.GetCommentRepliesCountAsync(Id);

                if (Replies != null) return Json(new { success = true, result = Replies, count = Count, skippedCount = SkipCount, loadedCount = Replies != null ? Replies.Count : 0 });
                else return Json(new { success = true, count = 0, skippedCount = 0, loadedCount = Replies != null ? Replies.Count : 0 });
            }
            else return Json(new { success = false, alert = "No found replies for this comment", count = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int MessageId, int UserId)
        {
            int Result = await _messageRepository.CheckAsync(MessageId, UserId);
            if (Result != 0) return Json(new { success = true, count = Result, id = MessageId });
            else return Json(new { success = false, alert = "You have no messages for marking as read for now" });
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageInfo(int Id, bool IsFromSender)
        {
            GetMessages_ViewModel? MsgInfo = await _messageRepository.GetMessageInfo(Id, IsFromSender);
            if (MsgInfo != null)
            {
                if (IsFromSender)
                {
                    if (MsgInfo.UserId != -256)
                    {
                        MsgInfo.ReceiverName = await _userRepository.GetUserPseudonameOrSearchnameById(MsgInfo.UserId, false);
                        MsgInfo.ReceiverSearchName = await _userRepository.GetUserPseudonameOrSearchnameById(MsgInfo.UserId, true);
                    }
                    else MsgInfo.ReceiverName = "Supports";
                }
                return Json(new { success = true, result = MsgInfo });
            }
            else return Json(new { success = false, alert = "Selected message info wasn't found" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForAdmins(int Id, bool GetRemovedToo, int SkipCount, int LoadCount)
        {
            IQueryable<GetMessages_ViewModel>? Messages_Preview = await _messageRepository.GetAdminsReceivedMessagesAsync(Id, GetRemovedToo, SkipCount, LoadCount);
            if (Messages_Preview != null)
            {
                List<GetMessages_ViewModel>? Messages = await Messages_Preview.ToListAsync();
                if (Messages != null) return Json(new { success = true, result = Messages, count = Messages.Count, skippedCount = SkipCount + Messages.Count, fullLoadedCount = SkipCount + Messages.Count });
                else return Json(new { success = false, alert = "There are no messages to load" });
            }
            else return Json(new { success = false, alert = "Unable to get messages. Please, try again later" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                IQueryable<GetMessages_ViewModel>? MessagesPreview = _messageRepository.GetReceivedMessages(Id, false);
                if (MessagesPreview != null)
                {
                    List<GetMessages_ViewModel>? Messages = await MessagesPreview.ToListAsync();
                    int MessagesCount = Messages.Count;
                    if (MessagesCount > 0) return Json(new { success = true, count = MessagesCount, result = Messages, userId = Id });
                    else return Json(new { success = true, count = MessagesCount, userId = Id });
                }
                else return Json(new { success = false, alert = "No messages found" });
            }
            else return Json(new { success = false, alert = "Sign in or log in to get full access of application" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSent(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                IQueryable<GetMessages_ViewModel>? GetAllSentMessages = _messageRepository.GetSentMessages(Id, false);
                if (GetAllSentMessages != null)
                {
                    List<GetMessages_ViewModel>? Result = await GetAllSentMessages.ToListAsync();
                    int ResultCount = Result.Count;
                    if (Result != null) return Json(new { success = true, result = Result, count = ResultCount });
                    else return Json(new { success = true, count = 0 });
                }
                else return Json(new { success = false, alert = "You haven't sent any message yet" });
            }
            return Json(new { success = false, alert = "Unable to get your messages now" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSentMessage(int Id, int UserId)
        {
            int Result = 0;
            if (UserId == -256) Result = await _messageRepository.RemoveForAdminsAsync(Id);
            else Result = await _messageRepository.RemoveAsync(Id, UserId);
            if (Result != 0) return Json(new { success = true, alert = "Selected message has been successfully deleted", id = Result });
            return Json(new { success = false, alert = "Unable to remove selected message" });
        }

        [HttpPost]
        public async Task<IActionResult> SendComment(SendComment_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                int Result = await _messageRepository.SendCommentAsync(Model);
                if (Result != 0) return Json(new { success = true, date = DateTime.Now, id = Result });
                else return Json(new { success = false, alert = "Unable to send comment for this project. Please, check entered text length, may be the problem is in it" });
            }
            else return Json(new { success = false, alert = "Unable to send comment for this project. Unexpected error" });
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(SendComment_ViewModel Model)
        {
            if(ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                string? Result = await _messageRepository.EditCommentAsync(Model);
                if (Result != null) return Json(new { success = true, result = Result });
            }
            return Json(new { success = false, alert = "Unable to edit selected comment. Please, try again later and be sure that that selected comment is yours" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveComment(int Id, int ProjectId, int UserId)
        {
            int Result = await _messageRepository.RemoveCommentAsync(Id, ProjectId, UserId);
            if (Result != 0) return Json(new { success = true, alert = "Your comment has been successfully removed", result = Result });
            else return Json(new { success = false, alert = "Unable to remove this comment. Please, try again later" });
        }


        [HttpGet]
        public async Task<IActionResult> GetComments(int ProjectId, int SkipCount, int Count)
        {
            IQueryable<GetCommentaries_ViewModel>? Result_Preview = _messageRepository.GetComments(ProjectId, SkipCount, Count);
            if (Result_Preview != null)
            {
                List<GetCommentaries_ViewModel>? Result = await Result_Preview.ToListAsync();
                int CommentsCount = await _messageRepository.GetProjectCommentsCountAsync(ProjectId);

                return Json(new { success = true, result = Result, count = CommentsCount, loadedCount = Count, skippedCount = SkipCount });
            }
            else return Json(new { success = false, alert = "Unable to get comments of this project at this moment. Please, try again later" });
        }
    }
}
