using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class MentionController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IMention _mentionRepository;
        
        public MentionController(Context context, UserManager<User> userManager, IMention mentionRepository)
        {
            _context = context;
            _userManager = userManager;
            _mentionRepository = mentionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMentions(int Id, int SkipCount, int LoadCount)
        {
            IQueryable<Mention>? Mentions_Preview = _mentionRepository.GetPostMentions(Id, SkipCount, LoadCount);
            if (Mentions_Preview != null)
            {
                List<Mention>? Mentions = await Mentions_Preview.ToListAsync();
                if (Mentions != null)
                {
                    return Json(new { success = true, result = Mentions, skippedCount = SkipCount, loadedCount = SkipCount + Mentions.Count, count = await _mentionRepository.GetMentionsCountAsync(Id) });
                }
            }
            return Json(new { success = false, alert = "We're sorry, but we haven't found any mention for this post" });
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendComment_ViewModel SendMentionModel)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                string? Result = await _mentionRepository.SendMentionAsync(SendMentionModel);
                if (Result != null) return Json(new { success = true, result = Result });
                else return Json(new { success = false, alert = "This post is private, so you can't send mentions for it" });
            }
            else return Json(new { success = false, alert = "Something went wrong. Please, check all datas and then try to send your mention for this post again" });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id, int PostId, int UserId)
        {
            int Result = await _mentionRepository.RemoveMentionAsync(Id, PostId, UserId);
            if (Result != 0) return Json(new { success = true, result = Result, alert = "Selected mention has been successfully removed" });
            else return Json(new { success = false, alert = "We're sorry, but an unexpected error has occured. Please, try to remove your mention later" });
        }
    }
}
