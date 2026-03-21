using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class LoanApplication
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public decimal LoanAmount { get; set; }

        [Required]
        public string LoanPurpose { get; set; }

        [Required]
        public int DurationMonths { get; set; }

        public decimal InterestRate { get; set; }

        public int? LoanTypeId { get; set; }

        public string Status { get; set; } = "Pending";

        public bool DocumentsRequested { get; set; } = false;

        public string? DocumentRequestNote { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Notification flags
        public bool IsReadByAdmin { get; set; } = false;
        public bool IsReadByUser { get; set; } = true;

        // Relationships
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("LoanTypeId")]
        public LoanType? LoanType { get; set; }

        public ICollection<Document> Documents { get; set; }

        public Loan Loan { get; set; }
    }
}