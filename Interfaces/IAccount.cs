using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IAccount
    {
        public Task<bool> QueueForVerificationAsync(int Id);
        public Task<int> VerifyUserAsync(int Id);
        public Task<VerificationQueue?> HasTheVerificationBeenSent(int Id);
        public Task<int> SendVerificationEmailAsync(string Id);
        public Task<string?> SendTemporaryCodeAsync(int Id, string Email, bool NeedsUniqueCode);
        public Task<bool> UpdatePasswordAsync(UpdatePassword_ViewModel Model);
        public Task<bool> ChangePasswordAsync(PasswordChange_ViewModel Model);
        public Task<bool> VerifyEmailAsync(VerifyEmail_ViewModel Model);
        public Task<bool> ChangeEmailViaReserveCodeAsync(ChangeEmail_ViewModel Model);
        public Task<string?> ChangeEmailViaOldEmailAsync(ChangeEmail_ViewModel Model);
    }
}
