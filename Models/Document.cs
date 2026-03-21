using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required]
        public int LoanApplicationId { get; set; }

        [Required]
        public string DocumentType { get; set; }

        [Required]
        public string FilePath { get; set; }

        public string? FileName { get; set; }

        public long FileSize { get; set; }

        public string? MimeType { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        public string? ReviewNotes { get; set; }

        public string? ReviewedBy { get; set; }

        // Relationship
        [ForeignKey("LoanApplicationId")]
        public LoanApplication LoanApplication { get; set; }
    }
}