using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class LoanDisbursement
    {
        public int Id { get; set; }

        [Required]
        public int LoanApplicationId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public int? BankId { get; set; }

        [StringLength(100)]
        public string? AccountName { get; set; }

        [StringLength(30)]
        public string? AccountNumber { get; set; }

        [StringLength(20)]
        public string? SortCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ApprovedAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }

        public DisbursementStatus Status { get; set; } = DisbursementStatus.PendingSetup;

        [StringLength(100)]
        public string? PaymentReference { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Notification flags
        public bool IsReadByAdmin { get; set; } = false;
        public bool IsReadByUser { get; set; } = true;

        // Navigation Properties
        [ForeignKey("LoanApplicationId")]
        public LoanApplication LoanApplication { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey("BankId")]
        public Bank? Bank { get; set; }
    }
}
