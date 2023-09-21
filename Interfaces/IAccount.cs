using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IAccount
    {
        public Task<string?> SendTemporaryCodeAsync(int Id, string Email, bool NeedsUniqueCode);
        public Task<bool> UpdatePasswordAsync(UpdatePassword_ViewModel Model);
        public Task<bool> ChangePasswordAsync(PasswordChange_ViewModel Model);
        public Task<bool> VerifyEmailAsync(VerifyEmail_ViewModel Model);
        public Task<bool> ChangeEmailViaReserveCodeAsync(ChangeEmail_ViewModel Model);
        public Task<string?> ChangeEmailViaOldEmailAsync(ChangeEmail_ViewModel Model);
    }
}
