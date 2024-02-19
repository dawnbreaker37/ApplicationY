using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class PaymentHistory
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? SentFromCard { get; set; }
        public string? SentToCard { get; set; }
        public int UserId { get; set; }
        public bool IsAbleToReturn { get; set; }
        [ForeignKey("User")]
        public int? SenderId { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public User? User { get; set; }
        public Project? Project { get; set; }
    }
}
