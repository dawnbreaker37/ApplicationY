﻿using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IAccount
    {
        public Task<bool> QueueForVerificationAsync(int Id);
        public Task<int> VerifyUserAsync(int Id);
        public Task<VerificationQueue?> HasTheVerificationBeenSent(int Id);
        public Task<int> SendVerificationEmailAsync(string Id);
        public Task<string?> SendTemporaryCodeAsync(int Id, string? Email, bool NeedsUniqueCode);
        public Task<(string?, string?)> CheckTheTemporaryCodeAsync(int Id, string Code);
        public Task<bool> UpdatePasswordAsync(UpdatePassword_ViewModel Model);
        public Task<bool> ChangePasswordAsync(PasswordChange_ViewModel Model);
        public Task<bool> VerifyEmailAsync(VerifyEmail_ViewModel Model);
        public Task<bool> ChangeEmailViaReserveCodeAsync(ChangeEmail_ViewModel Model);
        public Task<string?> ChangeEmailViaOldEmailAsync(ChangeEmail_ViewModel Model);
        public Task<int> DisableOrEnableAccountAsync(int Id, int SenderId, string Description);
        public Task<string?> ChangeUserRoleAsync(int Id, int ChangerId, int RoleId);
        public Task<User?> GetCurrentUserFromCacheAsync(int Id);
    }
}
