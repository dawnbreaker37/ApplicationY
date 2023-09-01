using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IUser : IBase<User>
    {
        public Task<User?> GetUserByIdAsync(int Id, bool NeedsLargerInfo);
        public Task<string?> CreateUserAsync(SignIn_ViewModel Model);
        public Task<bool> LogInAsync(LogIn_ViewModel Model);
        public Task<bool> IsUserNameUniqueAsync(string UserName);
        public Task<string?> SubmitAccountByReserveCodeAsync(string? Username, string? Email, string ReserveCode);
        public Task<string?> SubmitEmailByUniqueCodeAsync(int Id, string? Email, string Code);
        public IQueryable<User>? GetUserBySearchName(string SearchName);
    }
}
