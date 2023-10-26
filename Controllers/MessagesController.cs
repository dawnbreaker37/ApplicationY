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
        private readonly IMessage _messageRepository;

        public MessagesController(Context context, UserManager<User> userManager, IMessage messageRepository)
        {
            _context = context;
            _userManager = userManager;
            _messageRepository = messageRepository;
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
        public async Task<IActionResult> SendReply(SendReply_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _messageRepository.SendCommentReplyAsync(Model);
                if (Result) return Json(new { success = true, text = Model.Text, messageId = Model.CommentId });                
            }
            return Json(new { success = false, alert = "An error occured while trying to send your reply. Please, check all datas and then try again" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReplies(int Id)
        {
            IQueryable<GetReplies_ViewModel>? Replies_Preview = _messageRepository.GetReplies(Id);
            if (Replies_Preview != null)
            {
                List<GetReplies_ViewModel>? Replies = await Replies_Preview.ToListAsync();
                int Count = Replies.Count;

                if (Replies != null) return Json(new { success = true, result = Replies, count = Count });
                else return Json(new { success = true, count = 0 });
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
        public async Task<IActionResult> GetMessageInfo(int Id)
        {
            GetMessages_ViewModel? MsgInfo = await _messageRepository.GetMessageInfo(Id);
            if (MsgInfo != null) return Json(new { success = true, result = MsgInfo });
            else return Json(new { success = false, alert = "Selected message info wasn't found" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                IQueryable<GetMessages_ViewModel>? MessagesPreview = _messageRepository.GetAllMyMessages(Id, false);
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
                IQueryable<GetMessages_ViewModel>? GetAllSentMessages = _messageRepository.GetAllMyMessages(Id, true);
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
            int Result = await _messageRepository.RemoveAsync(Id, UserId);
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

        [HttpGet]
        public async Task<IActionResult> GetComments(int ProjectId)
        {
            IQueryable<GetCommentaries_ViewModel>? Result_Preview = _messageRepository.GetComments(ProjectId);
            if (Result_Preview != null)
            {
                List<GetCommentaries_ViewModel>? Result = await Result_Preview.ToListAsync();
                int Count = Result.Count;

                return Json(new { success = true, result = Result, Count = Count });
            }
            else return Json(new { success = false, alert = "Unable to get comments of this project at this moment. Please, try again later" });
        }
    }
}
