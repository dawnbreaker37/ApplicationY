using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IUser _userRepository;
        private readonly IAccount _accountRepository;
        private readonly IMailService _mailServiceRepository;

        public AccountController(Context context, UserManager<User> userManager, IUser userRepository, IAccount accountRepository, IMailService mailServiceRepository)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
            _accountRepository = accountRepository; 
            _mailServiceRepository = mailServiceRepository;
        }

        public IActionResult Create(bool SigningIn)
        {
            ViewBag.Reason = SigningIn;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                string? Result = await _userRepository.CreateUserAsync(Model);
                if (Result != null) return Json(new { success = true, alert = "Account successfully created", code = Result });
            }
            return Json(new { success = false, alert = "An error occured while trying to create account. Please, check entered datas and try again" });
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogIn_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _userRepository.LogInAsync(Model);
                if (Result) return Json(new { success = true, alert = "You've logged in successfully. You'll be relocated to main page in a moment" });
            }
            return Json(new { success = false, alert = "Wrong username/email or password. Please, try again" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePassword_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _accountRepository.UpdatePasswordAsync(Model);
                if (Result) return Json(new { success = true, alert = "Your password has successfully changed. Now, you can enter by your new password" });
                else return Json(new { success = false, alert = "Something went wrong while we tried to reset your password. Please, check all datas and be sure that entered passwords are same and contain [8-24] digits, and then, try it again" });
            }
            return Json(new { success = false, alert = "Passowords must contain [8-24] digits and be equal to each other" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword_First(PasswordChange_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if (UserInfo != null && !String.IsNullOrEmpty(Model.CurrentPassword))
                {
                    bool Result = await _userManager.CheckPasswordAsync(UserInfo, Model.CurrentPassword);
                    if (Result) return Json(new { success = true, currentPassword = Model.CurrentPassword, newPassword = Model.NewPassword, userId = UserInfo.Id, alert = "Password is correct. Now, let's confirm the new one and submit it all by reserve code" });
                    else return Json(new { success = false, alert = "Password is incorrent. Please, try again. May be you've entered wrong password" });
                }
            }
            return Json(new { success = false, alert = "An error occured while checking your account. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword_Final(PasswordChange_ViewModel Model)
        {
            if(ModelState.IsValid)
            {
                User? UserInfo = await _userManager.FindByIdAsync(Model.UserId.ToString());
                Model.UserInfo = UserInfo;
                bool Result = await _accountRepository.ChangePasswordAsync(Model);

                if (Result) return Json(new { success = true, alert = "Your password has successfully changed" });
                else return Json(new { success = false, alert = "An error occured while trying to change your password. May be it's reserve code problem. Please, try again it later" });
            }
            return Json(new { success = false, alert = "Wrong new entered password or reserve code. Please, check it and try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRecovering(string Username, bool ByUsername, string ReserveCode)
        {
            string? Result;
            if (ByUsername) Result = await _userRepository.SubmitAccountByReserveCodeAsync(Username, null, ReserveCode);
            else Result = await _userRepository.SubmitAccountByReserveCodeAsync(null, Username, ReserveCode);

            if (Result != null) return Json(new { success = true, token = Result, reserveCode = ReserveCode, alert = "Account submitted successfully. Now you can create your new password" });
            else return Json(new { success = false, alert = "Reserve code or email/username are wrong. Please, try it again" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmailViaReserveCode(ChangeEmail_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _accountRepository.ChangeEmailViaReserveCodeAsync(Model);
                if (Result) return Json(new { success = true, email = Model.NewEmail, alert = "Your email has successfully changed" });
                else return Json(new { success = false, alert = "An error occured while trying to change your email. May be you've entered wrong password or reserve code. Anyway, please, try it again later" });
            }
            return Json(new { success = false, alert = "An error occured while trying to change your email" });
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailConfirmationCode(int Id, string Email)
        {
            string? Result = await _accountRepository.SendTemporaryCodeAsync(Id, Email, true);
            if (Result != null)
            {
                SendEmail_ViewModel Model = new SendEmail_ViewModel
                {
                    Subject = "Email Confirmation Code",
                    Title = "Email Confirmation Code",
                    ToEmail = Email,
                    Body = "Hi! Here we present You Your email verification code. Enter it in the page to verify your account and get some improved features as a present for You from us! Thank You and good luck! </br> <h3 style='color: #0d6efd;'>" + Result + "</h3>"
                };
                MailKit_ViewModel KitModel = new MailKit_ViewModel();
                bool Result2 = await _mailServiceRepository.SendEmailAsync(Model, KitModel);
                string? Token = await _userRepository.SubmitEmailByUniqueCodeAsync(Id, Email, Result);

                if (Result2 && Token != null)
                {
                    return Json(new { success = true, result = Token, id = Id, email = Email, alert = "Your verification code have successfully sent. Please, check your email and enter it in the next widget" });
                }
                else return Json(new { success = false, alert = "Something went wrong while trying to send your verification code. Please, try again later" });
            }
            else return Json(new { success = false, alert = "Entered email and account aren't equal. Please check your email. If the problem isn't solved, try it again later" });
        }

        [HttpPost]
        public async Task<IActionResult> SendCode(string Email)
        {
            string? Text;
            string? ReserveCode = await _accountRepository.SendTemporaryCodeAsync(0, Email, false);
            if (ReserveCode != null)
            {
                User? UserInfo = await _context.Users.AsNoTracking().Select(u => new User { Email = u.Email, PseudoName = u.PseudoName }).FirstOrDefaultAsync(u => u.Email == Email);
                if (UserInfo != null) Text = "Hello, <h5 style='font-weight: 500;'>" + UserInfo.PseudoName + "</h5>! Here's your reserve code for recovering the password of your account. We reccoment you to save this code somewhere for next times. Good luck! <div style='margin-top: 20px;'></div> <h3 style='font-weight: 500; color: #0d6efd;'>" + ReserveCode + "</h3>";
                else Text = "Hello! Here's your reserve code for recovering the password of your account. We reccoment you to save this code somewhere for next times. Good luck! <div style='margin-top: 20px;'></div> <h3 style='font-weight: 500; color: #0d6efd;'>" + ReserveCode + "</h3>";

                MailKit_ViewModel KitModel = new MailKit_ViewModel();
                SendEmail_ViewModel Model = new SendEmail_ViewModel
                {
                    Body = Text,
                    FromEmail = "bluejade@mail.ru",
                    Subject = "Reserve Code Of Your Account",
                    Title = "Reserve Code Of Your Account",
                    ToEmail = Email
                };
                bool Result2 = await _mailServiceRepository.SendEmailAsync(Model, KitModel);

                if (Result2) return Json(new { success = true, email = UserInfo?.Email, text = "We've sent you your reserve code to your email. Please, check your mailbox and enter it in next widget" });
            }
            return Json(new { success = false, text = "An error occured while sending email. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> EditMainInfo(EditAccount_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _userRepository.EditUserInfoAsync(Model);
                if (Result) return Json(new { success = true, result = Model, alert = "Your account info has edited successfully" });
                else return Json(new { success = false, alert = "An error occured while editing your account. Please, check all entered datas and try again" });
            }
            return Json(new { success = false, alert = "Some entered datas are wrong. Please, check them, and try to edit again" });
        }

        [HttpPost]
        public async Task<IActionResult> EditDescription(EditDescription_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _userRepository.EditDescriptionAsync(Model);
                if (Result) return Json(new { success = true, description = Model.Description, alert = "Your account description has successfully edited" });
            }
            return Json(new { success = false, alert = "An error occured while trying to edit your account description. Please, check all datas and try it again" });
        }

        [HttpPost]
        public async Task<IActionResult> EditLinks(EditLinks_ViewModel Model)
        {
            if(ModelState.IsValid)
            {
                bool Result = await _userRepository.EditLinksAsync(Model);
                if (Result) return Json(new { success = true, alert = "Links have successfully updated recently", link1 = Model.Link1, link2 = Model.Link2, link1Tag = Model.Link1Tag, link2Tag = Model.Link2Tag });
            }
            return Json(new { success = false, alert = "Something went wrong while we tried to edit/add links to your account. Please, check all your entered datas and try again. All fields need to be entered and they can't be empty" });
        }

        [HttpGet]
        public async Task<IActionResult> IsSearchNameUnique(int Id, string SearchName)
        {
            bool Result = await _userRepository.IsSearchNameUniqueAsync(Id, SearchName);
            if (Result) return Json(new { success = false, alert = "Searchname isn't free. Please, try to use another one" });
            else return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> IsUserNameUnique(string UserName)
        {
            if (!String.IsNullOrEmpty(UserName) && UserName.Length > 3)
            {
                bool Result = await _userRepository.IsUserNameUniqueAsync(UserName);
                if (Result) return Json(new { success = false });
                else return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
