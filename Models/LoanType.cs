using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models
{
    public class LoanType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal InterestRate { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<LoanApplication>? LoanApplications { get; set; }
    }
}
