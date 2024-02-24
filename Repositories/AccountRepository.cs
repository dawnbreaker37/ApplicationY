using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationY.Repositories
{
    public class AccountRepository : Base<User>, IAccount
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly INotifications _notificationsRepository;
        private readonly IOthers _othersRepository;
        private readonly IMemoryCache _cache;
        public AccountRepository(Context context, UserManager<User> userManager, IMemoryCache cache, IOthers others, INotifications notificationsRepository) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _notificationsRepository = notificationsRepository;
            _othersRepository = others;
            _cache = cache;
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
                        bool IsNewEmailUnique = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == Model.NewEmail);
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

        public async Task<VerificationQueue?> HasTheVerificationBeenSent(int Id)
        {
            return await _context.VerificationQueues.AsNoTracking().Select(q => new VerificationQueue { SentAt = q.SentAt, UserId = q.UserId, VerifiedAt = q.VerifiedAt }).FirstOrDefaultAsync(q => q.UserId == Id);
        }

        public async Task<int> SendVerificationEmailAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> VerifyUserAsync(int Id)
        {
            if(Id != 0)
            {
                int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsVerified, true));
                if (Result != 0)
                {
                    int InfoChangeResult = await _context.VerificationQueues.AsNoTracking().Where(u => u.UserId == Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.VerifiedAt, DateTime.Now));
                    if (InfoChangeResult != 0) return Id;
                }
            }
            return 0;
        }

        public async Task<bool> QueueForVerificationAsync(int Id)
        {
            bool HaveQueuedYet = await _context.VerificationQueues.AsNoTracking().AnyAsync(q => q.UserId == Id);
            if (!HaveQueuedYet)
            {
                VerificationQueue verificationQueue = new VerificationQueue()
                {
                    SentAt = DateTime.Now,
                    UserId = Id,
                    VerifiedAt = null
                };
                await _context.AddAsync(verificationQueue);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<string?> SendTemporaryCodeAsync(int Id, string? Email, bool NeedsUniqueCode)
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
                        _cache.Remove("uniqueCode_" + Id);
                        _cache.Set("uniqueCode_" + Id, Code, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12)));

                        return Code;
                    }
                }
                else
                {
                    if (Id != 0)
                    {
                        bool IsEmailEqualToId = await _context.Users.AsNoTracking().AnyAsync(u => u.Id == Id && u.Email == Email);
                        if (IsEmailEqualToId)
                        {
                            Code = Guid.NewGuid().ToString("D").Substring(0, 8);
                            _cache.Remove("uniqueCode_" + Id);
                            _cache.Set("uniqueCode_" + Id, Code, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12)));

                            return Code;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<(string?, string?)> CheckTheTemporaryCodeAsync(int Id, string Code)
        {
            if(Id != 0 || Code != null)
            {
                bool Result = _cache.TryGetValue("uniqueCode_" + Id, out string? ConfirmationCode);
                if(Result && ConfirmationCode == Code)
                {
                    _cache.Remove("uniqueCode_" + Id);
                    User? UserInfo = await _userManager.FindByIdAsync(Id.ToString());
                    if(UserInfo != null)
                    {
                        return (await _userManager.GeneratePasswordResetTokenAsync(UserInfo), UserInfo.ReserveCode);
                    }
                }
            }
            return (null, null);
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
                        UserInfo.PasswordResetDate = DateTime.Now;
                        SendNotification_ViewModel SendNotificationModel = new SendNotification_ViewModel
                        {
                            Title = "Password Updated",
                            Description = "Your password has been updated at " + DateTime.Now.ToShortDateString() + ", at " + DateTime.Now.ToShortTimeString() + ".",
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
                bool AreEmailAndIdEqual = await _context.Users.AsNoTracking().AnyAsync(u => u.Id == Model.Id && u.Email == Model.Email);
                if (AreEmailAndIdEqual)
                {
                    bool IsThereAnyCodeFromThisUser = _cache.TryGetValue("uniqueCode_" + Model.Id, out string? Code);
                    if (IsThereAnyCodeFromThisUser && Model.Code == Code)
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

        public async Task<int> DisableOrEnableAccountAsync(int Id, int SenderId, string Description)
        {
            if(Id != 0 && SenderId != 0)
            {
                int SenderRoleId = await _othersRepository.GetUserRoleAsync(SenderId);
                if(SenderRoleId > 0)
                {
                    bool IsAccountDisabled = await _context.Users.AsNoTracking().AnyAsync(u => u.IsDisabled);
                    if (!IsAccountDisabled)
                    {
                        int TargetRoleId = await _othersRepository.GetUserRoleAsync(Id);
                        if (TargetRoleId < SenderRoleId)
                        {
                            if ((!String.IsNullOrEmpty(Description)) && Description.Length <= 1000 && Description.Length >= 40)
                            {
                                await _context.Users.Where(u => u.Id == Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsDisabled, true));
                                DisabledAccount disabledAccount = new DisabledAccount
                                {
                                    Description = Description,
                                    DisabledAt = DateTime.Now,
                                    SenderId = SenderId,
                                    UserId = Id
                                };
                                await _context.AddAsync(disabledAccount);
                                await _context.SaveChangesAsync();

                                return Id;
                            }
                        }
                    }
                    else
                    {
                        await _context.Users.Where(u => u.Id == Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsDisabled, false));
                        int Result = await _context.DisabledAccounts.AsNoTracking().Where(u => u.UserId == Id && u.SenderId == SenderId).ExecuteDeleteAsync();
                        if(Result != 0) { return Id; }
                    }
                }
            }
            return 0;
        }

        public async Task<string?> ChangeUserRoleAsync(int Id, int ChangerId, int RoleId)
        {
            if (Id != 0 && ChangerId != 0 && RoleId != 0)
            {
                int CurrentUserRole = await _othersRepository.GetUserRoleAsync(Id);
                int ChangerRole = await _othersRepository.GetUserRoleAsync(ChangerId);
                if (ChangerRole >= CurrentUserRole)
                {
                    string? NeededRoleName = await _othersRepository.GetRoleNameAsync(RoleId);
                    User? UserInfo = await _userManager.FindByIdAsync(Id.ToString());
                    if (UserInfo != null && NeededRoleName != null)
                    {
                        IdentityResult? Result = await _userManager.AddToRoleAsync(UserInfo, NeededRoleName);
                        if (Result.Succeeded) return NeededRoleName;
                    }
                }
            }
            return null;
        }

        public async Task<User?> GetCurrentUserFromCacheAsync(int Id)
        {
            bool Result = _cache.TryGetValue("user_" + Id, out User? UserInfo);
            if(Result)
            {
                if (UserInfo != null)
                {
                    return UserInfo;
                }
            }
            else
            {
                UserInfo = await _userManager.FindByIdAsync(Id.ToString());
                _cache.Set("user_" + Id, UserInfo, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12)));
                return UserInfo;
            }
            return null;
        }
    }
}
