﻿using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace ApplicationY.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUser _userRepository;
        private readonly IAccount _accountRepository;
        private readonly IMailService _mailServiceRepository;
        private readonly IMemoryCache _cache;

        public AccountController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IUser userRepository, IAccount accountRepository, IMailService mailServiceRepository, IMemoryCache cache)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _cache = cache;
            _accountRepository = accountRepository; 
            _mailServiceRepository = mailServiceRepository;
        }

        public IActionResult Create(bool SigningIn)
        {
            ViewBag.Reason = SigningIn;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindUserExistence(string UsernameOrEmail)
        {
            User? UserInfo = await _userRepository.CheckUserEntryTypeByUsernameOrEmail(UsernameOrEmail);
            if (UserInfo != null && UserInfo.AccessFailedCount < 5)
            {
                if (UserInfo.IsEasyEntryEnabled)
                {
                    string? Code = await _accountRepository.SendTemporaryCodeAsync(UserInfo.Id, UserInfo.Email, true);
                    if (Code != null)
                    {
                        MailKit_ViewModel KitModel = new MailKit_ViewModel();
                        SendEmail_ViewModel Model = new SendEmail_ViewModel
                        {
                            Body = "<div style='border: 2px solid rgb(240, 240, 240); border-radius: 8px; padding: 2px 5px 2px 5px; text-align: center;'><h2 style='color: #0d6efd; font-family: 'Trebuchet MS;'>" + Code + "</h2><div style='margin-top: 6px;'></div><p style='color: rgb(240, 240, 240);'>Here's your one-time 8-digit code to enter into your account via <span style='color: #0d6efd;'>Easy Entry</span></p><div style='border-top: 1px solid rgb(240, 240, 240); margin-top: 1px; padding-top: 1px;'></div><p style='color: brown;'>If this message was <span style='font-weight: 500;'>not</span> triggered by you, please immediately enter to your account, disable easy entry and change your password</p></div>",
                            FromEmail = "bluejade@mail.ru",
                            Subject = "Easy Entry Code",
                            Title = "Easy Entry Code",
                            ToEmail = UserInfo.Email
                        };
                        bool Result = await _mailServiceRepository.SendEmailAsync(Model, KitModel);
                        if (Result) return Json(new { success = true, result = UserInfo, easyEntryEnabled = true });
                        else return Json(new { success = false, alert = "We're sorry, but an unexpected error has been occured. We're unable to send an email. Please, try to send a message to our supports if you've checked all datas and they're true" });
                    }
                }
                else return Json(new { success = true, result = UserInfo, easyEntryEnabled = false });
            }
            return Json(new { success = false });
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
                int AccessFailedCount = await _userRepository.GetAccessFailedCountByEmailOrUserNameAsync(Model.UserName);
                if (AccessFailedCount < 5)
                {
                    bool Result = await _userRepository.LogInAsync(Model);
                    if (Result) return Json(new { success = true, alert = "You've logged in successfully. You'll be relocated to main page in a moment" });
                    else
                    {
                        if(AccessFailedCount >= 4)
                        {
                            (string?, string?) ReserveCodeAndEmail = await _userRepository.GetReserveCodeAndEmailByEmailOrUserNameAsync(Model.UserName);
                            MailKit_ViewModel kitModel = new MailKit_ViewModel();
                            SendEmail_ViewModel sendEmailModel = new SendEmail_ViewModel()
                            {
                                Title = "Your Password Has Been Reset",
                                FromEmail = "bluejade@mail.ru",
                                Body = "<div style='border: 2px solid rgb(240, 240, 240); border-radius: 8px; padding: 2px; text-align: center;'><p style='font-weight: 500; font-size: small;'>Hi! We're informing you that your account has been disabled for <span color='#0d6efd;'>several mins</span> due to a suspitious activity from it. Someone has tried many times to enter into your account, so, to secure your account we've disabled it for a several time. Note that you'll still be able to enter to it if you haven't logged out before. In such cases we recommend to reset the password of account for your own safety.</p><div style='border-top: 1px solid rgb(240, 240, 240); margin-top: 2px; padding-top: 2px;'></div><p>Here's your <span style='font-weight: 500;'>reserve code</span> in case if you want to reset the password of your account. Thank you <h1 style='margin-top: 4px; color: #0d6efd;'>" + ReserveCodeAndEmail.Item1 + "</h1></div>",
                                Subject = Model.UserName,
                                ToEmail = ReserveCodeAndEmail.Item2
                            };
                            await _mailServiceRepository.SendEmailAsync(sendEmailModel, kitModel);
                        }
                    }
                }
                else return Json(new { success = false, accessFailedCount = AccessFailedCount, alert = "You've exceeded the limit of incorrect entries. Your access to this account has been limited" });
            }
            return Json(new { success = false, accessFailedCount = 0, alert = "Wrong username/email or password. Please, try again" });
        }

        [HttpPost]
        public async Task<IActionResult> LogInViaCode(LogIn_ViewModel Model)
        {
            bool Result = await _userRepository.LogInViaCodeAsync(Model);
            if (Result) return Json(new { success = true, alert = "You've logged in successfully. You'll be relocated to main page in a moment" });
            else return Json(new { success = false, alert = "Wrong username/email or one-time code. Please, check all datas and try again" });
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePassword_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _accountRepository.UpdatePasswordAsync(Model);
                if (Result) return Json(new { success = true, alert = "Your password has been successfully changed. Now, you can enter by your new password" });
                else return Json(new { success = false, alert = "Something went wrong while we tried to reset your password. Please, check all datas and be sure that entered passwords are same and contain [8-24] digits, and then, try it again" });
            }
            return Json(new { success = false, alert = "Passwords must contain [8-24] digits and be equal to each other" });
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
                string? Token = await _userRepository.SubmitEmailByUniqueCodeAsync(Id, Email);

                if (Result2 && Token != null)
                {
                    return Json(new { success = true, result = Token, id = Id, email = Email, alert = "Your verification code have successfully sent. Please, check your email and enter it in the next widget" });
                }
                else return Json(new { success = false, alert = "Something went wrong while trying to send your verification code. Please, try again later" });
            }
            else return Json(new { success = false, alert = "Entered email and account aren't equal. Please check your email. If the problem isn't solved, try it again later" });
        }

        [HttpPost]
        public async Task<IActionResult> SendCode(string Email, bool NeedUniqueCode, int Id)
        {
            string? Text;
            string? ReserveCode = await _accountRepository.SendTemporaryCodeAsync(Id, Email, NeedUniqueCode);
            if (ReserveCode != null)
            {
                User? UserInfo = await _context.Users.AsNoTracking().Select(u => new User { Email = u.Email, PseudoName = u.PseudoName }).FirstOrDefaultAsync(u => u.Email == Email);
                if (!NeedUniqueCode)
                {
                    if (UserInfo != null) Text = "Hello, <h5 style='font-weight: 500;'>" + UserInfo.PseudoName + "</h5>! Here's your code to recover your password. We reccomend you to save this code somewhere for next times. Good luck! <div style='margin-top: 20px;'></div> <h3 style='font-weight: 500; color: #0d6efd;'>" + ReserveCode + "</h3>";
                    else Text = "Hello! Here's your code for your account. We reccomend you to save this code somewhere for next times. Good luck! <div style='margin-top: 20px;'></div> <h3 style='font-weight: 500; color: #0d6efd;'>" + ReserveCode + "</h3>";
                }
                else
                {
                    Text = "Hi! You've received a one-time code for your account and here's it. Please, enter it and continue your action in the app. Thank you! <div style='margin-top: 8px; border-radius: 8px; background-color: transparent; border: 2px solid rgb(240, 240, 240); text-align: center; padding: 4px;'><h3>Attached Code</h3><h2 style='margin-top: 12px; color: #0d6efd;'>" + ReserveCode + "</h2></div>";
                }

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
        public async Task<IActionResult> ConfirmTheCode(int Id, string Code)
        {
            (string?, string?) Result = await _accountRepository.CheckTheTemporaryCodeAsync(Id, Code);
            if (Result.Item1 != null && Result.Item2 != null)
            {
                return Json(new { success = true, alert = "You've confirmed the code successfully", token = Result.Item1, reserveCode = Result.Item2 });
            }
            else return Json(new { success = false, alert = "Entered code is not correct. Please, try again" });
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
        public async Task<IActionResult> EditEasyEntry(int Id, bool IsEasyEntryEnabled)
        {
            if (User.Identity.IsAuthenticated)
            {
                string? CurrentUserId = _userManager.GetUserId(User);
                if (CurrentUserId == Id.ToString())
                {
                    int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Id && !u.IsDisabled).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsEasyEntryEnabled, IsEasyEntryEnabled));
                    if (Result != 0)
                    {
                        if(IsEasyEntryEnabled) return Json(new { success = true, alert = "You've enabled <span class='fw-500'>easy entry</span>" });
                        else return Json(new { success = true, alert = "Easy Entry is disabled. This will allow your to sign-in by using an one time code that we'll send to your email" });
                    }
                    else return Json(new { success = false, alert = "An unexpected error occured. Please, check all datas and then try again" });
                }
                else return Json(new { succcess = false, alert = "You are NOT able to edit any kind of information for other users" });
                //User? UserInfo = await _userManager.GetUserAsync(User);
                //if(UserInfo != null)
                //{
                //}
            }
            else return Json(new { success = false, alert = "You are not signed in. Please, sign in, to edit your account info" });
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
        public async Task<IActionResult> EditMainSettings(EditAccountMainSettings_ViewModel Model)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _userRepository.EditMainInfoAsync(Model);
                if (Result) return Json(new { success = true, result = Model });
                else return Json(new { success = false, alert = "Unable to edit main settings. Please, try again later" });
            }
            else return Json(new { success = false, alert = "An unexpected error occured. Please, try again later" });
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

        [HttpPost]
        public async Task<IActionResult> EditProfilePhoto(int Id, IFormFile ProfilePhoto)
        {
            if (ModelState.IsValid)
            {
                string? Result = await _userRepository.EditProfilePhotoAsync(Id, ProfilePhoto);
                if (Result != null) return Json(new { success = true, alert = "Profile photo has been successfully updated", url = Result });
                else return Json(new { success = false, alert = "An error occured while trying to update your profile photo. Please, try again later" });
            }
            else return Json(new { success = false, alert = "Unable to load selected photo. May be it's too heavy. Anyway, please, try with another photo again" });
        }

        [HttpPost]
        public async Task<IActionResult> EditPersonalInfo(EditPersonalInfo_ViewModel Model)
        {
            //if(Model.CreatedAt.Year == 1) Model.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                //Model.CreatedAt = null;
                string? Result = await _userRepository.EditPersonalInfoAsync(Model);
                if (Result != null) return Json(new { success = true, alert = "Your personal information has successfully updated", result = Model, country = Result });
                else return Json(new { success = false, alert = "An error occured while trying to update your personal information. Please, check correctness of entered datas and try again. Also check the correctness of entered data value (d/m/yyyy), for example: 1/12/2020" });
            }
            else return Json(new { success = false, alert = "An error occured while trying to update your personal information. Please, check all datas and try again" });
        }

        [HttpPost]
        public async Task<IActionResult> QueueForVerification(int Id)
        {
            bool Result = await _accountRepository.QueueForVerificationAsync(Id);
            if (Result) return Json(new { success = true, time = DateTime.Now, alert = "You've got in line for the verification. Please, wait, it may take few days before you get the email. When you'll get the email of verification, please, go by the link in that email to instantly verify your account. Thanks for your patience!" });
            else return Json(new { success = false, alert = "You've already been queued for the verification. Please, wait until you'll get the email message for verification. Thanks for your patience" });
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

        [HttpPost]
        public async Task<IActionResult> Disable(int Id, string Description)
        {
            string? CurrentUserId = _userManager.GetUserId(User);
            int CurrentUserId_Int32;
            bool TryParseResult = Int32.TryParse(CurrentUserId, out CurrentUserId_Int32);

            if (TryParseResult)
            {
                int Result = await _accountRepository.DisableOrEnableAccountAsync(Id, CurrentUserId_Int32, Description);
                if (Result != 0) return Json(new { success = true, alert = "Selected account has been disabled", id = Id });
            }
            return Json(new { success = false, alert = "An error occured. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(int Id, int RoleId)
        {
            string? ChangerId = _userManager.GetUserId(User);
            int ChangerUserId_Int32;
            bool TryParseResult = Int32.TryParse(ChangerId, out ChangerUserId_Int32);
            if (TryParseResult)
            {
                string? Result = await _accountRepository.ChangeUserRoleAsync(Id, ChangerUserId_Int32, RoleId);
                if (Result != null) return Json(new { success = true, alert = "Selected user's role has been successfully updated", roleName = Result, userId = Id });
            }
            return Json(new { success = false, alert = "Unable to change selected user's role" });
        }
    }
}
