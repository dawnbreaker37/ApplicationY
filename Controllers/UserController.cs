using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationY.Controllers
{
    public class UserController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUser _userRepository;
        private readonly IAccount _accountRepository;

        public UserController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IAccount accountRepository, IUser userRepository)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null)
                {
                    ViewBag.UserInfo = UserInfo;

                    return View();
                }
                else return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Create", "Account");
        }

        public async Task<IActionResult> Verify(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userRepository.GetUserByIdAsync(Id, true);
                ViewBag.UserInfo = UserInfo;

                return View();
            }
            return RedirectToAction("Create", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyEmail_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _accountRepository.VerifyEmailAsync(Model);
                if (Result) return Json(new { success = true, alert = "Congrats! You've verified your email successfully. Now, you have secured your account and for that you've received some improved features of this application. More about by clicking the button" });

            }
            return Json(new { success = false, alert = "Something went wrong while trying to confirm your email. Please, try again later" });
        }
    }
}
