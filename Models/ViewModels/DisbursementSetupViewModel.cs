using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models.ViewModels
{
    public class DisbursementSetupViewModel
    {
        public int DisbursementId { get; set; }

        public int LoanApplicationId { get; set; }

        [Required(ErrorMessage = "Please select a bank.")]
        [Display(Name = "Bank")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(30)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Sort Code")]
        public string? SortCode { get; set; }

        public decimal ApprovedAmount { get; set; }

        public IEnumerable<Bank>? Banks { get; set; }
    }
}
