using System;
using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models.ViewModels
{
    public class EligibilityCheckViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Employment status is required.")]
        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }

        [Required(ErrorMessage = "Monthly income is required.")]
        [Range(0, 10000000, ErrorMessage = "Please enter a valid income.")]
        [Display(Name = "Monthly Income ($)")]
        public decimal MonthlyIncome { get; set; }

        [Required(ErrorMessage = "Monthly expenses are required.")]
        [Range(0, 10000000, ErrorMessage = "Please enter a valid amount.")]
        [Display(Name = "Monthly Expenses ($)")]
        public decimal MonthlyExpenses { get; set; }

        [Required(ErrorMessage = "Credit score range is required.")]
        [Display(Name = "Credit Score Range")]
        public CreditScoreRange CreditScoreRange { get; set; }

        [Required(ErrorMessage = "Please select a loan type.")]
        [Display(Name = "Loan Type")]
        public int LoanTypeId { get; set; }

        [Required(ErrorMessage = "Desired loan amount is required.")]
        [Range(100, 10000000, ErrorMessage = "Loan amount must be at least $100.")]
        [Display(Name = "Desired Loan Amount ($)")]
        public decimal DesiredLoanAmount { get; set; }

        [Required(ErrorMessage = "Loan duration is required.")]
        [Range(1, 360, ErrorMessage = "Duration must be between 1 and 360 months.")]
        [Display(Name = "Loan Duration (Months)")]
        public int DurationMonths { get; set; }
    }
}
