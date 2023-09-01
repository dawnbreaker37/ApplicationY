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
        public AccountRepository(Context context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
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
                    if (Result.Succeeded) return true;
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
