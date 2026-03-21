using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class TicketMessage
    {
        public int Id { get; set; }

        [Required]
        public int SupportTicketId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsAdminReply { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("SupportTicketId")]
        public SupportTicket SupportTicket { get; set; }

        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }
    }
}
