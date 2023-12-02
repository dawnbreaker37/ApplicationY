using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IUser : IBase<User>
    {
        public Task<GetUserInfo_ViewModel?> GetUserBySearchnameAsync(string? Searchname);
        public Task<GetUserInfo_ViewModel?> GetUserByIdAsync(int Id, bool NeedsLargerInfo, bool NeedPhoto);
        public Task<string?> CreateUserAsync(SignIn_ViewModel Model);
        public Task<bool> EditUserInfoAsync(EditAccount_ViewModel Model);
        public Task<bool> EditDescriptionAsync(EditDescription_ViewModel Model);
        public Task<bool> EditLinksAsync(EditLinks_ViewModel Model);
        public Task<string?> EditPersonalInfoAsync(EditPersonalInfo_ViewModel Model);
        public Task<string?> EditProfilePhotoAsync(int Id, IFormFile File);
        public Task<bool> LogInAsync(LogIn_ViewModel Model);
        public Task<bool> IsUserNameUniqueAsync(string UserName);
        public Task<bool> IsSearchNameUniqueAsync(int Id, string SearchName);
        public Task<string?> SubmitAccountByReserveCodeAsync(string? Username, string? Email, string ReserveCode);
        public Task<string?> SubmitEmailByUniqueCodeAsync(int Id, string? Email, string Code);
        public IQueryable<User>? GetUserBySearchName(string SearchName);
        public IQueryable<GetUserInfo_ViewModel> GetRandomUsers(int MaxCount);
        public IQueryable<GetUserInfo_ViewModel>? FindUsers(string Keyword);
        public string? LinkIconModifier(string? LinkTag);
    }
}
