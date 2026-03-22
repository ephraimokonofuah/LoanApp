using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class Repayment
    {
        public int Id { get; set; }

        [Required]
        public int LoanId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public int InstallmentNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrincipalPortion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InterestPortion { get; set; }

        public DateTime DueDate { get; set; }

        public RepaymentStatus Status { get; set; } = RepaymentStatus.Upcoming;

        public PaymentMethodType PaymentMethodRequested { get; set; } = PaymentMethodType.None;

        public DateTime? PaymentRequestedAt { get; set; }

        [StringLength(1000)]
        public string? PaymentDetails { get; set; }

        public DateTime? PaymentDetailsSentAt { get; set; }

        public DateTime? PaidAt { get; set; }

        [StringLength(100)]
        public string? TransactionReference { get; set; }

        // Notification flags
        public bool IsReadByAdmin { get; set; } = false;
        public bool IsReadByUser { get; set; } = true;

        // Relationships
        [ForeignKey("LoanId")]
        public Loan Loan { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
    }
}