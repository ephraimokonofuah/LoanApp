using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int LoanApplicationId { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal InterestRate { get; set; }

        public int DurationMonths { get; set; }

        public DateTime StartDate { get; set; }

        public string Status { get; set; } = "Active";

        // Relationships
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("LoanApplicationId")]
        public LoanApplication LoanApplication { get; set; }

        public ICollection<Repayment> Repayments { get; set; }
    }
}