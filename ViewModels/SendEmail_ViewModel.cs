using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class SendEmail_ViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string? FromEmail { get; set; } = "bluejade@mail.ru";
        [DataType(DataType.EmailAddress)]
        public string? ToEmail { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Subject { get; set; }
    }
}
