using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public enum EmploymentStatus
    {
        Employed,
        [Display(Name = "Self-Employed")]
        SelfEmployed,
        Unemployed
    }

    public enum CreditScoreRange
    {
        Excellent,
        Good,
        Fair,
        Poor
    }

    public class EligibilityCheck
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public EmploymentStatus EmploymentStatus { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyIncome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyExpenses { get; set; }

        [Required]
        public CreditScoreRange CreditScoreRange { get; set; }

        [Required]
        public int LoanTypeId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DesiredLoanAmount { get; set; }

        [Required]
        public int DurationMonths { get; set; }

        // Calculated results
        public bool IsEligible { get; set; }
        public string? EligibilityReason { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyRepayment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRepayment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalInterest { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; }

        // Workflow
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public string? AdminNotes { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Notification flags
        public bool IsReadByAdmin { get; set; } = false;
        public bool IsReadByUser { get; set; } = true;

        // Navigation
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("LoanTypeId")]
        public LoanType? LoanType { get; set; }
    }
}
