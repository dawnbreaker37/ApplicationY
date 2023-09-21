using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class UserRepository : Base<User>, IUser
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(Context context, UserManager<User> userManager, SignInManager<User> signInManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string?> CreateUserAsync(SignIn_ViewModel Model)
        {
            if (Model.Email != null && Model.Password != null && Model.UserName != null)
            {
                bool AreEmailAndUserNameUnique = await _context.Users.AnyAsync(u => (u.UserName == null || u.UserName.ToLower() == Model.UserName.ToLower()) || (u.Email == null || u.Email.ToLower() == Model.Email.ToLower()));
                if (!AreEmailAndUserNameUnique)
                {
                    string? RandomId = Guid.NewGuid().ToString("D").Substring(0, 6);
                    string? ReserveCode = Guid.NewGuid().ToString("D").Substring(0, 6);
                    User NewUser = new User
                    {
                        SearchName = "id-" + RandomId,
                        UserName = Model.UserName,                      
                        Email = Model.Email,
                        ReserveCode = ReserveCode,
                        PseudoName = Model.UserName,
                        CreatedAt = null
                    };

                    IdentityResult? Result = await _userManager.CreateAsync(NewUser, Model.Password);
                    if (Result.Succeeded)
                    {
                        SignInResult? SignInResult = await _signInManager.PasswordSignInAsync(Model.UserName, Model.Password, true, true);
                        if (SignInResult.Succeeded)
                        {
                            return ReserveCode;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> EditDescriptionAsync(EditDescription_ViewModel Model)
        {
            if(Model.Id != 0 && !String.IsNullOrEmpty(Model.Description))
            {
                int Result = await _context.Users.Where(u => u.Id == Model.Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.Description, Model.Description));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<bool> EditLinksAsync(EditLinks_ViewModel Model)
        {
            if((Model.Id) != 0 && (!String.IsNullOrEmpty(Model.Link1) && !String.IsNullOrEmpty(Model.Link2)))
            {
                int Result = await _context.Users.Where(u => u.Id == Model.Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.Link1, Model.Link1).SetProperty(u => u.Link1Tag, Model.Link1Tag).SetProperty(u => u.Link2, Model.Link2).SetProperty(u => u.Link2Tag, Model.Link2Tag));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<bool> EditUserInfoAsync(EditAccount_ViewModel Model)
        {
            if (Model != null && !String.IsNullOrEmpty(Model.SearchName))
            {
                bool IsSearchNameFree = await _context.Users.AnyAsync(u => (u.Id != Model.Id) && (u.SearchName != null && u.SearchName.ToLower() == Model.SearchName.ToLower()));
                if (!IsSearchNameFree)
                {
                    int Result = await _context.Users.Where(u => u.SearchName != null && u.SearchName.ToLower() == Model.RealSearchName.ToLower()).ExecuteUpdateAsync(u => u.SetProperty(u => u.PseudoName, Model.PseudoName).SetProperty(u => u.SearchName, Model.SearchName).SetProperty(u => u.Description, Model.Description));
                    if (Result != 0) return true;
                }
            }
            return false;
        }

        public async Task<User?> GetUserByIdAsync(int Id, bool NeedLargerInfo)
        {
            if(Id != 0)
            {
                if (NeedLargerInfo) return await _context.Users.AsNoTracking().Select(u => new User { Id = u.Id, PseudoName = u.PseudoName, UserName = u.UserName, SearchName = u.SearchName, Email = u.Email, EmailConfirmed = u.EmailConfirmed }).FirstOrDefaultAsync(u => u.Id == Id);
                else return await _context.Users.AsNoTracking().Select(u => new User { Id = u.Id, PseudoName = u.PseudoName, UserName = u.UserName, SearchName = u.SearchName }).FirstOrDefaultAsync(u => u.Id == Id);
            }
            return null;
        }

        public IQueryable<User>? GetUserBySearchName(string SearchName)
        {
            if (SearchName != null) return _context.Users.AsNoTracking().Where(u => (u.SearchName == null) || (u.SearchName.ToLower().Contains(SearchName.ToLower()))).Select(u => new User { Id = u.Id, UserName = u.UserName, SearchName = u.SearchName });
            else return null;
        }

        public async Task<bool> IsSearchNameUniqueAsync(int Id, string SearchName)
        {
            if(Id != 0 && !String.IsNullOrEmpty(SearchName))
            {
                bool Result = await _context.Users.AsNoTracking().AnyAsync(u => (u.Id != Id) && (u.SearchName != null && u.SearchName.ToLower() == SearchName.ToLower()));
                if(Result) return true;
            }
            return false;
        }

        public async Task<bool> IsUserNameUniqueAsync(string UserName)
        {
            bool Result = await _context.Users.AsNoTracking().AnyAsync(u => u.UserName == null || u.UserName.ToLower() == UserName.ToLower());
            return Result;
        }

        public async Task<bool> LogInAsync(LogIn_ViewModel Model)
        {
            if (Model.UserName != null && Model.Password != null)
            {
                SignInResult? Result = await _signInManager.PasswordSignInAsync(Model.UserName, Model.Password, true, true);
                if (Result.Succeeded) return true;
                else
                {
                    User? UserInfo = await _userManager.FindByEmailAsync(Model.UserName);
                    if (UserInfo != null)
                    {
                        Result = await _signInManager.PasswordSignInAsync(UserInfo, Model.Password, true, true);
                        if(Result.Succeeded) return true;
                    }
                }
            }
            return false;
        }

        public async Task<string?> SubmitAccountByReserveCodeAsync(string? Username, string? Email, string ReserveCode)
        {
            if(!String.IsNullOrEmpty(ReserveCode))
            {
                User? UserInfo;
                if(!String.IsNullOrEmpty(Username) && String.IsNullOrEmpty(Email))
                {
                    UserInfo = await _userManager.FindByNameAsync(Username);
                    //UserInfo = await _context.Users.Select(u => new User { Id = u.Id, ReserveCode = u.ReserveCode, UserName = u.UserName, Email = u.Email }).AsNoTracking().FirstOrDefaultAsync(u => u.UserName == null || u.UserName == Username);
                    if (UserInfo != null && UserInfo.ReserveCode == ReserveCode)
                    {
                        string? TokenProvider = await _userManager.GeneratePasswordResetTokenAsync(UserInfo);
                        return TokenProvider;
                    }
                }
                else if(!String.IsNullOrEmpty(Email) && String.IsNullOrEmpty(Username))
                {
                    UserInfo = await _userManager.FindByEmailAsync(Email);
                    //UserInfo = await _context.Users.Select(u => new User { Id = u.Id, ReserveCode = u.ReserveCode, UserName = u.UserName, Email = u.Email }).AsNoTracking().FirstOrDefaultAsync(u => u.Email == null || u.Email == Email);
                    if (UserInfo != null && UserInfo.ReserveCode == ReserveCode)
                    {
                        string? TokenProvider = await _userManager.GeneratePasswordResetTokenAsync(UserInfo);
                        return TokenProvider;
                    }
                }
            }
            return null;
        }

        public async Task<string?> SubmitEmailByUniqueCodeAsync(int Id, string? Email, string Code)
        {
            if(Id != 0 && Email != null)
            {
                User? UserInfo = await _userManager.FindByIdAsync(Id.ToString());
                if (UserInfo != null)
                {
                    string? Token = await _userManager.GenerateEmailConfirmationTokenAsync(UserInfo);
                    return Token;
                }
            }
            return null;
        }
    }
}
