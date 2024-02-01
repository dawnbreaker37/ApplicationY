using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationY.Repositories
{
    public class UserRepository : Base<User>, IUser
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserRepository(Context context, UserManager<User> userManager, IMemoryCache cache, IWebHostEnvironment webHostEnvironment, SignInManager<User> signInManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _cache = cache;
            _webHostEnvironment = webHostEnvironment;
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
                            IdentityUserRole<int> role = new IdentityUserRole<int>()
                            {
                                RoleId = 1,
                                UserId = NewUser.Id
                        };
                            await _context.AddAsync(role);
                            await _context.SaveChangesAsync();

                            return ReserveCode;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<string?> EditCountryInfoAsync(int Id, int CountryId)
        {
            if(Id != 0 && CountryId != 0)
            {
                int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.CountryId, CountryId));
                if (Result != 0) return await _context.Countries.AsNoTracking().Where(c => c.Id == CountryId).Select(c => c.ISO + ", " + c.Name).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<bool> EditDescriptionAsync(EditDescription_ViewModel Model)
        {
            if(Model.Id != 0 && !String.IsNullOrEmpty(Model.Description))
            {
                int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Model.Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.Description, Model.Description));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<bool> EditLinksAsync(EditLinks_ViewModel Model)
        {
            if(Model.Id != 0)
            {
                int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Model.Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.Link1, Model.Link1).SetProperty(u => u.Link1Tag, Model.Link1Tag).SetProperty(u => u.Link2, Model.Link2).SetProperty(u => u.Link2Tag, Model.Link2Tag));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<string?> EditPersonalInfoAsync(EditPersonalInfo_ViewModel Model)
        {
            if(Model.Id != 0 && Model.CountryId != 0)
            {
                int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Model.Id).ExecuteUpdateAsync(u => u.SetProperty(u=>u.CountryId, Model.CountryId).SetProperty(u => u.IsCompany, Model.IsCompany).SetProperty(u => u.CreatedAt, Model.CreatedAt));
                if (Result != 0 && Model.CountryId != 0) return await _context.Countries.AsNoTracking().Where(c => c.Id == Model.CountryId).Select(c => c.ISO + ", " + c.Name).FirstOrDefaultAsync();
                else return "No country";
            }
            return null;
        }

        public async Task<string?> EditProfilePhotoAsync(int Id, IFormFile File)
        {
            if(Id != 0 && File != null)
            {
                string? Extension = Path.GetExtension(File.FileName);
                string? NewFileName = Guid.NewGuid().ToString("D").Substring(0, 12);
                string? FileFullName = string.Concat(NewFileName, Extension);
                
                using(FileStream fs = new FileStream(_webHostEnvironment.WebRootPath + "/ProfilePhotos/" + FileFullName, FileMode.Create))
                {
                    await File.CopyToAsync(fs);
                    int Result = await _context.Users.AsNoTracking().Where(u => u.Id == Id).ExecuteUpdateAsync(u => u.SetProperty(u => u.ProfilePhoto, FileFullName));
                    if (Result != 0) return FileFullName;
                }
            }
            return null;
        }

        public async Task<bool> EditUserInfoAsync(EditAccount_ViewModel Model)
        {
            if (Model != null && !String.IsNullOrEmpty(Model.SearchName))
            {
                bool IsSearchNameFree = await _context.Users.AnyAsync(u => (u.Id != Model.Id) && (u.SearchName != null && u.SearchName.ToLower() == Model.SearchName.ToLower()));
                if (!IsSearchNameFree)
                {
                    int Result = await _context.Users.AsNoTracking().Where(u => u.SearchName != null && Model.RealSearchName != null && u.SearchName.ToLower() == Model.RealSearchName.ToLower()).ExecuteUpdateAsync(u => u.SetProperty(u => u.PseudoName, Model.PseudoName).SetProperty(u => u.SearchName, Model.SearchName));
                    if (Result != 0) return true;
                }
            }
            return false;
        }

        public IQueryable<GetUserInfo_ViewModel>? FindUsers(string Keyword, bool NeedAdditionalInfo)
        {
            if (Keyword != null)
            {
                if (!NeedAdditionalInfo) return _context.Users.AsNoTracking().Where(u => u.SearchName!.ToLower().Contains(Keyword.ToLower()) || u.PseudoName!.ToLower().Contains(Keyword) || u.Email!.ToLower().Contains(Keyword.ToLower()) || u.Description != null && u.Description.ToLower().Contains(Keyword.ToLower()) || u.Country != null && u.Country.Name != null && u.Country.Name.ToLower().Contains(Keyword.ToLower())).Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, SearchName = u.SearchName, CountryFullName = u.Country!.Name, ProjectsCount = u.Projects == null ? 0 : u.Projects.Count(p => !p.IsRemoved) });
                else return _context.Users.AsNoTracking().Where(u => u.SearchName!.ToLower().Contains(Keyword.ToLower()) || u.PseudoName!.ToLower().Contains(Keyword.ToLower()) || u.Email!.ToLower().Contains(Keyword.ToLower()) || (u.UserName != null && u.UserName.ToLower().Contains(Keyword.ToLower()))).Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, SearchName = u.SearchName });
            }
            else return null;
        }

        public IQueryable<GetUserInfo_ViewModel> GetRandomUsers(int MaxCount)
        {
            if (MaxCount != 0) return _context.Users.AsNoTracking().Where(u=> u.Projects != null && u.Projects.Count != 0).Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, SearchName = u.SearchName, Description = u.Description, IsCompany = u.IsCompany, ProjectsCount = u.Projects == null ? 0 : u.Projects.Count, CountryFullName = u.Country!.Name }).OrderByDescending(u => Guid.NewGuid()).Take(MaxCount);
            else return _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, SearchName = u.SearchName, Description = u.Description, ProjectsCount = u.Projects == null ? 0 : u.Projects.Count, IsCompany = u.IsCompany, CountryFullName = u.Country!.Name }).OrderByDescending(u => Guid.NewGuid()).Take(10);
        }

        public async Task<GetUserInfo_ViewModel?> GetUserByIdAsync(int Id, bool NeedLargerInfo, bool NeedPhoto, bool ForAdmins)
        {
            if(Id != 0)
            {
                bool Result = false;
                if (!ForAdmins)
                {
                    if (NeedLargerInfo)
                    {
                        if (NeedPhoto) return await _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, UserName = u.UserName, SearchName = u.SearchName, ProfilePhoto = u.ProfilePhoto, Description = u.Description, CreatedAt = u.CreatedAt, Email = u.Email, IsEmailConfirmed = u.EmailConfirmed, ProjectsCount = u.Projects != null ? u.Projects.Count : 0, SubscribersCount = u.Subscribtions != null ? u.Subscribtions.Count : 0, Country = new Country { Name = u.Country!.Name } }).FirstOrDefaultAsync(u => u.Id == Id);
                        else return await _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, UserName = u.UserName, SearchName = u.SearchName, Description = u.Description, CreatedAt = u.CreatedAt, Email = u.Email, IsEmailConfirmed = u.EmailConfirmed, ProjectsCount = u.Projects != null ? u.Projects.Count : 0, SubscribersCount = u.Subscribtions != null ? u.Subscribtions.Count : 0, Country = new Country { Name = u.Country!.Name } }).FirstOrDefaultAsync(u => u.Id == Id);
                    }
                    else
                    {
                        Result = _cache.TryGetValue("user_" + Id, out User? UserInfo);
                        if (!Result)
                        {
                            _cache.Set("user_" + Id, await _userManager.FindByIdAsync(Id.ToString()), new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(12)));
                            return await _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, SearchName = u.SearchName, Description = u.Description, CountryFullName = u.Country!.ISO + ", " + u.Country!.Name }).FirstOrDefaultAsync(u => u.Id == Id);
                        }
                        else
                        {
                            if(UserInfo != null) return new GetUserInfo_ViewModel { Id = UserInfo.Id, PseudoName = UserInfo.PseudoName, SearchName = UserInfo.SearchName, Description = UserInfo.Description, CountryFullName = UserInfo.Country!.ISO + ", " + UserInfo.Country!.Name };
                            else return await _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, SearchName = u.SearchName, Description = u.Description, CountryFullName = u.Country!.ISO + ", " + u.Country!.Name }).FirstOrDefaultAsync(u => u.Id == Id);
                        }
                    }
                }
                else return await _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, PseudoName = u.PseudoName, UserName = u.UserName, IsDisabled = u.IsDisabled, SearchName = u.SearchName, Email = u.Email, IsEmailConfirmed = u.EmailConfirmed, IsVerifiedAccount = u.IsVerified, ProjectsCount = u.Projects != null ? u.Projects.Count : 0, RemovedProjectsCount = u.Projects != null ? u.Projects.Count(p => p.IsRemoved) : 0 }).FirstOrDefaultAsync(u => u.Id == Id);
            }
            return null;
        }

        public IQueryable<User>? GetUserBySearchName(string SearchName)
        {
            if (SearchName != null) return _context.Users.AsNoTracking().Where(u => (u.SearchName == null) || (u.SearchName.ToLower().Contains(SearchName.ToLower()))).Select(u => new User { Id = u.Id, UserName = u.UserName, SearchName = u.SearchName });
            else return null;
        }

        public async Task<GetUserInfo_ViewModel?> GetUserBySearchnameAsync(string? Searchname)
        {
            if (!String.IsNullOrEmpty(Searchname)) return await _context.Users.AsNoTracking().Select(u => new GetUserInfo_ViewModel { Id = u.Id, Email = u.Email, Link1 = u.Link1, Link1Tag = u.Link1Tag, Link2 = u.Link2, Link2Tag = u.Link2Tag, CreatedAt = u.CreatedAt, IsVerifiedAccount = u.IsVerified, Description = u.Description, PseudoName = u.PseudoName, SearchName = u.SearchName, IsCompany = u.IsCompany, CountryFullName = u.Country != null ? u.Country!.Name : null, ProfilePhoto = u.ProfilePhoto }).FirstOrDefaultAsync(u => u.SearchName == null || u.SearchName.ToLower() == Searchname.ToLower());
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

        public string? LinkIconModifier(string? LinkTag)
        {
            if (!String.IsNullOrEmpty(LinkTag))
            {
                string NewLinkTag = LinkTag.ToLower();
                if (NewLinkTag.Contains("twitter"))
                {
                    NewLinkTag = " <i class='fab fa-twitter text-primary'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("x"))
                {
                    NewLinkTag = " <i class='fab fa-twitter text-primary'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("facebook"))
                {
                    NewLinkTag = " <i class='fab fa-facebook-square text-primary'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("instagram"))
                {
                    NewLinkTag = " <i class='fab fa-instagram'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("vk"))
                {
                    NewLinkTag = " <i class='fab fa-vk text-primary'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("telegram"))
                {
                    NewLinkTag = "<i class='fab fa-telegram text-primary'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("whatsapp"))
                {
                    NewLinkTag = " <i class='fab fa-whatsapp text-success'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("viber"))
                {
                    NewLinkTag = " <i class='fab fa-viber' id='style: color: rebeccapurple;'></i> " + LinkTag;
                }
                else if (NewLinkTag.Contains("youtube"))
                {
                    NewLinkTag = " <i class='fab fa-youtube text-danger'></i> " + LinkTag;
                }
                else NewLinkTag = " <i class='fas fa-globe text-primary'></i> " + LinkTag;
                return NewLinkTag;
            }
            return null;
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

        public async Task<string?> SubmitEmailByUniqueCodeAsync(int Id, string? Email)
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
