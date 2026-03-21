using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models.ViewModels
{
    public class DisbursementUpdateViewModel
    {
        public int DisbursementId { get; set; }

        public int LoanApplicationId { get; set; }

        public string ApplicantName { get; set; } = string.Empty;

        public string? BankName { get; set; }

        public string? AccountName { get; set; }

        public string? AccountNumber { get; set; }

        public string? SortCode { get; set; }

        public decimal ApprovedAmount { get; set; }

        public DisbursementStatus CurrentStatus { get; set; }

        [Required(ErrorMessage = "Please select a new status.")]
        [Display(Name = "New Status")]
        public DisbursementStatus NewStatus { get; set; }

        [Display(Name = "Paid Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Paid amount must be greater than 0.")]
        public decimal? PaidAmount { get; set; }

        [StringLength(100)]
        [Display(Name = "Payment Reference")]
        public string? PaymentReference { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}
