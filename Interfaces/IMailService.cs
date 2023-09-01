using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IMailService
    {
        public Task<bool> SendEmailAsync(SendEmail_ViewModel Model, MailKit_ViewModel KitModel);
    }
}
