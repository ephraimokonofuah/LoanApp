using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? Country { get; set; }

        public string? PostCode { get; set; }

        public bool IsBanned { get; set; } = false;

        public string? BanReason { get; set; }

        public DateTime? BannedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}