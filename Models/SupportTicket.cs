using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public enum TicketCategory
    {
        General = 0,
        LoanApplication = 1,
        Disbursement = 2,
        AccountIssue = 3,
        DocumentUpload = 4,
        PaymentIssue = 5,
        TechnicalSupport = 6,
        Other = 7
    }

    public enum TicketPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Urgent = 3
    }

    public enum TicketStatus
    {
        Open = 0,
        InProgress = 1,
        AwaitingResponse = 2,
        Resolved = 3,
        Closed = 4
    }

    public class SupportTicket
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(150)]
        public string Subject { get; set; }

        [Required]
        public string TicketNumber { get; set; }

        public TicketCategory Category { get; set; } = TicketCategory.General;

        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public string? AssignedToId { get; set; }

        public bool IsReadByAdmin { get; set; } = false;
        public bool IsReadByUser { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("AssignedToId")]
        public ApplicationUser? AssignedTo { get; set; }

        public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }
}
