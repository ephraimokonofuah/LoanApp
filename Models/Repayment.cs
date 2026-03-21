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
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }

        public string Reference { get; set; }

        // Relationship
        [ForeignKey("LoanId")]
        public Loan Loan { get; set; }
    }
}