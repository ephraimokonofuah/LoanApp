using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models
{
    public class SiteSettings
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Site Name")]
        [StringLength(100)]
        public string SiteName { get; set; } = "LoanApp";

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = "support@loanapp.com";

        [Display(Name = "Secondary Email")]
        [EmailAddress]
        [StringLength(150)]
        public string? SecondaryEmail { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(30)]
        public string Phone { get; set; } = "(800) 555-LOAN";

        [Display(Name = "Secondary Phone")]
        [StringLength(30)]
        public string? SecondaryPhone { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(200)]
        public string AddressLine1 { get; set; } = "123 Finance Street, Suite 400";

        [Display(Name = "Address Line 2")]
        [StringLength(200)]
        public string? AddressLine2 { get; set; } = "New York, NY 10001";

        [Display(Name = "Business Hours")]
        [StringLength(100)]
        public string? BusinessHours { get; set; } = "Mon – Fri: 8AM – 6PM EST";

        [Display(Name = "Footer Description")]
        [StringLength(500)]
        public string? FooterDescription { get; set; } = "Trusted lending solutions with competitive rates and transparent terms. We're committed to helping you achieve your financial goals with fast approvals and flexible repayment options.";

        [Display(Name = "Facebook URL")]
        [StringLength(200)]
        public string? FacebookUrl { get; set; }

        [Display(Name = "Twitter/X URL")]
        [StringLength(200)]
        public string? TwitterUrl { get; set; }

        [Display(Name = "LinkedIn URL")]
        [StringLength(200)]
        public string? LinkedInUrl { get; set; }

        [Display(Name = "Instagram URL")]
        [StringLength(200)]
        public string? InstagramUrl { get; set; }

        [Display(Name = "NMLS Number")]
        [StringLength(50)]
        public string? NmlsNumber { get; set; } = "123456";

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
