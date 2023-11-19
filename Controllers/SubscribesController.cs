using ApplicationY.Data;
using ApplicationY.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationY.Controllers
{
    public class SubscribesController : Controller
    {
        private readonly Context _context;
        private readonly ISubscribe _subscribeRepository;

        public SubscribesController(Context context, ISubscribe subscribeRepository)
        {
            _context = context;
            _subscribeRepository = subscribeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SubscribtionOption(int UserId, int SubscriberId, bool IsSubscribed)
        {
            if(IsSubscribed)
            {
                bool Result = await _subscribeRepository.DeclineSubscribtionAsync(UserId, SubscriberId);
                if (Result) return Json(new { success = true, result = false, alert = "Your subscribtion has been successfully canceled" });
            }
            else
            {
                bool Result = await _subscribeRepository.SubscribeAsync(UserId, SubscriberId);
                if (Result) return Json(new { success = true, result = true, alert = "You have been successfully subscribed" });
            }
            return Json(new { success = false, alert = "An error occured while trying to accept/cancel your subscribtion. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> SubscribtionOptionWithoutKnowing(int UserId, int SubscriberId)
        {
            bool IsSubscribed = await _subscribeRepository.IsUserSubscribed(UserId, SubscriberId);
            //if (IsSubscribed)
            //{
            //    bool Result = await _subscribeRepository.DeclineSubscribtionAsync(UserId, SubscriberId);
            //    if (Result) return Json(new { success = true, result = false, alert = "Your subscribtion has been successfully canceled" });
            //}
            //else
            //{
            //    bool Result = await _subscribeRepository.SubscribeAsync(UserId, SubscriberId);
            //    if (Result) return Json(new { success = true, result = true, alert = "You have been successfully subscribed" });
            //}
            Task<IActionResult>? Result = SubscribtionOption(UserId, SubscriberId, IsSubscribed);
            if(Result != null) return await Result;
            else return Json(new { success = false, alert = "An error occured while trying to accept/cancel your subscribtion. Please, try again later" });
        }
    }
}
