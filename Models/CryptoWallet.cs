using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models
{
    public class CryptoWallet
    {
        public int Id { get; set; }

        [Required]
        public PaymentMethodType WalletType { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Wallet Address")]
        public string WalletAddress { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Network")]
        public string? Network { get; set; } // e.g., TRC-20, ERC-20, BTC Mainnet

        [StringLength(100)]
        [Display(Name = "Label / Name")]
        public string? Label { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
