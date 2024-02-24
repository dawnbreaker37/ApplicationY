using ApplicationY.Interfaces;
using ApplicationY.ViewModels;
using MailKit.Net.Smtp;
using MimeKit;

namespace ApplicationY.Models
{
    public class SendEmailRepository : IMailService
    {
        public SendEmailRepository()
        {

        }

        public async Task<bool> SendEmailAsync(SendEmail_ViewModel Model, MailKit_ViewModel KitModel)
        {
            using (MimeMessage Message = new MimeMessage())
            {
                Message.Subject = Model.Subject;
                Message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = Model.Body
                };
                Message.Date = DateTime.Now;
                Message.From.Add(new MailboxAddress("YApp Reserve Code", KitModel.Mail));
                Message.To.Add(new MailboxAddress("", Model.ToEmail));
                
                using(SmtpClient SmtpClient = new SmtpClient())
                {
                    await SmtpClient.ConnectAsync(KitModel.Host, KitModel.Port);
                    await SmtpClient.AuthenticateAsync(KitModel.Mail, KitModel.Password);
                    await SmtpClient.SendAsync(Message);

                    await SmtpClient.DisconnectAsync(true);

                    return true;
                }
            }
        }
    }
}

