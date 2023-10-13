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
        public async Task<IActionResult> MarkAsRead(int MessageId, int UserId)
        {
            int Result = await _messageRepository.CheckAsync(MessageId, UserId);
            if (Result != 0) return Json(new { success = true, count = Result, id = MessageId });
            else return Json(new { success = false, alert = "You have no messages for marking as read for now" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                IQueryable<GetMessages_ViewModel>? MessagesPreview = _messageRepository.GetAllMyMessages(Id);
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
    }
}
