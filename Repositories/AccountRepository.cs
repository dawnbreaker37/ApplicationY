using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class AccountRepository : Base<User>, IAccount
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly INotifications _notificationsRepository;
        public AccountRepository(Context context, UserManager<User> userManager, INotifications notificationsRepository) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _notificationsRepository = notificationsRepository; 
        }

        public Task<string?> ChangeEmailViaOldEmailAsync(ChangeEmail_ViewModel Model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangeEmailViaReserveCodeAsync(ChangeEmail_ViewModel Model)
        {
            if(!String.IsNullOrEmpty(Model.Email) && !String.IsNullOrEmpty(Model.Password) && !String.IsNullOrEmpty(Model.NewEmail))
            {
                User? UserInfo = await _userManager.FindByEmailAsync(Model.Email);
                if(UserInfo != null)
                {
                    bool CheckPassword = await _userManager.CheckPasswordAsync(UserInfo, Model.Password);
                    if (CheckPassword && Model.ReserveCode == UserInfo.ReserveCode)
                    {
                        bool IsNewEmailUnique = await _context.Users.AnyAsync(u => u.Email == Model.NewEmail);
                        if (!IsNewEmailUnique)
                        {
                            string? ChangeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(UserInfo, Model.NewEmail);
                            IdentityResult? Result = await _userManager.ChangeEmailAsync(UserInfo, Model.NewEmail, ChangeEmailToken);
                            if (Result.Succeeded)
                            {
                                SendNotification_ViewModel NotificationModel = new SendNotification_ViewModel
                                {
                                    Title = "Email has been changed",
                                    UserId = UserInfo.Id,
                                    Description = "Your account email has recently changed. Actual date of changing is: " + DateTime.Now.ToShortDateString() + ", at " + DateTime.Now.ToShortTimeString() + ". From now your account's email can't be changed no more"
                                };
                                UserInfo.IsEmailChanged = true;
                                await _notificationsRepository.SendNotification(NotificationModel);

                                return true;
                            }
                        }
                    }
                    else
                    {
                        SendNotification_ViewModel NotificationModel = new SendNotification_ViewModel
                        {
                            Title = "Someone tried to change your email",
                            UserId = UserInfo.Id,
                            Description = "Someone has tried to change your account's email. Please, immediatly change your password if it's not you. Actual time of try: " + DateTime.Now.ToShortDateString() + ", at " + DateTime.Now.ToShortTimeString() + ". From now your account's email can't be changed no more"
                        };
                        await _notificationsRepository.SendNotification(NotificationModel);
                    }
                }
            }
            return false;
        }

        public async Task<bool> ChangePasswordAsync(PasswordChange_ViewModel Model)
        {
            if(Model.UserInfo != null && !String.IsNullOrEmpty(Model.NewPassword) && !String.IsNullOrEmpty(Model.ConfirmPassword) && !String.IsNullOrEmpty(Model.CurrentPassword))
            {
                if ((Model.NewPassword == Model.ConfirmPassword) && (Model.UserInfo.ReserveCode == Model.ReserveCode))
                {
                    IdentityResult? Result = await _userManager.ChangePasswordAsync(Model.UserInfo, Model.CurrentPassword, Model.NewPassword);
                    if (Result.Succeeded)
                    {
                        SendNotification_ViewModel NotificationModel = new SendNotification_ViewModel
                        {
                            Description = "Your password changed at " + DateTime.Now.ToShortDateString() + ", at " + DateTime.Now.ToShortTimeString() + ".",
                            Title = "Password changed",
                            UserId = Model.UserId
                        };
                        Model.UserInfo.PasswordResetDate = DateTime.Now;
                        await _notificationsRepository.SendNotification(NotificationModel);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<string?> SendTemporaryCodeAsync(int Id, string Email, bool NeedsUniqueCode)
        {
            if (!String.IsNullOrEmpty(Email))
            {
                    string? Code;
                    if (!NeedsUniqueCode)
                    {
                    User? UserInfo = await _userManager.Users.AsNoTracking().Select(u => new User { Id = u.Id, Email = u.Email, ReserveCode = u.ReserveCode }).FirstOrDefaultAsync(u => u.Email == Email);
                    if (UserInfo != null)
                    {
                        Code = UserInfo.ReserveCode;

                        await _context.TemporaryCodes.Where(u => u.UserId == UserInfo.Id).ExecuteDeleteAsync();
                        TemporaryCode temporaryCode = new TemporaryCode
                        {
                            UserId = UserInfo.Id,
                            Title = "Password recovering reserve code",
                            Code = Code,
                            SentDate = DateTime.Now
                        };
                        await _context.AddAsync(temporaryCode);
                        await _context.SaveChangesAsync();

                        return Code;
                    }
                }
                else
                {
                    if (Id != 0)
                    {
                        bool IsEmailEqualToId = await _context.Users.AnyAsync(u => u.Id == Id && u.Email == Email);
                        if (IsEmailEqualToId)
                        {
                            Code = Guid.NewGuid().ToString("D").Substring(0, 8);
                            await _context.TemporaryCodes.Where(u => u.UserId == Id).ExecuteDeleteAsync();

                            TemporaryCode temporaryCode = new TemporaryCode
                            {
                                Code = Code,
                                SentDate = DateTime.Now,
                                Title = "Email confirmation code",
                                UserId = Id
                            };
                            await _context.AddAsync(temporaryCode);
                            await _context.SaveChangesAsync();

                            return Code;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> UpdatePasswordAsync(UpdatePassword_ViewModel Model)
        {
            if((Model != null) && (Model.Token != null && Model.Password != null))
            {
                User? UserInfo;
                if (Model.SearchByUsername) UserInfo = await _userManager.FindByNameAsync(Model.EmailOrUsername!);                
                else UserInfo = await _userManager.FindByEmailAsync(Model.EmailOrUsername!);
                if(UserInfo != null)
                {
                    IdentityResult? Result = await _userManager.ResetPasswordAsync(UserInfo, Model.Token, Model.Password);
                    if (Result.Succeeded)
                    {
                        SendNotification_ViewModel SendNotificationModel = new SendNotification_ViewModel
                        {
                            Title = "Password updated",
                            Description = "Your password updated successfully at " + DateTime.Now.ToShortDateString() + ", at " + DateTime.Now.ToShortTimeString() + ".",
                            UserId = UserInfo.Id,
                        };
                        await _notificationsRepository.SendNotification(SendNotificationModel);

                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> VerifyEmailAsync(VerifyEmail_ViewModel Model)
        {
            if(Model != null && !String.IsNullOrEmpty(Model.Token))
            {
                bool AreEmailAndIdEqual = await _context.Users.AnyAsync(u => u.Id == Model.Id && u.Email == Model.Email);
                if (AreEmailAndIdEqual)
                {
                    bool IsThereAnyCodeLikeThis = await _context.TemporaryCodes.AnyAsync(u => u.UserId == Model.Id && u.Code == Model.Code);
                    if (IsThereAnyCodeLikeThis)
                    {
                        User? UserInfo = await _userManager.FindByIdAsync(Model.Id.ToString());
                        if (UserInfo != null)
                        {
                            IdentityResult? Result = await _userManager.ConfirmEmailAsync(UserInfo, Model.Token);
                            if (Result.Succeeded) return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
