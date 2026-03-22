using LoanApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Data

{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<EligibilityCheck> EligibilityChecks { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<LoanDisbursement> LoanDisbursements { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasData(

                new ApplicationUser
                {
                    Id = "seed-user-1",
                    FullName = "John Doe",
                    UserName = "john@example.com",
                    NormalizedUserName = "JOHN@EXAMPLE.COM",
                    Email = "john@example.com",
                    NormalizedEmail = "JOHN@EXAMPLE.COM",
                    Address = "123 Main Street",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                },

                new ApplicationUser
                {
                    Id = "seed-user-2",
                    FullName = "Mary Smith",
                    UserName = "mary@example.com",
                    NormalizedUserName = "MARY@EXAMPLE.COM",
                    Email = "mary@example.com",
                    NormalizedEmail = "MARY@EXAMPLE.COM",
                    Address = "45 Manchester Road",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                }

            );

            modelBuilder.Entity<LoanApplication>().HasData(

            new LoanApplication
            {
                Id = 1,
                UserId = "seed-user-1",
                LoanAmount = 5000,
                LoanPurpose = "Small Business Expansion",
                DurationMonths = 12,
                InterestRate = 8.5m,
                Status = "Pending",
                CreatedAt = new DateTime(2026, 1, 10)
            },

            new LoanApplication
            {
                Id = 2,
                UserId = "seed-user-1",
                LoanAmount = 12000,
                LoanPurpose = "Car Purchase",
                DurationMonths = 24,
                InterestRate = 9.2m,
                Status = "Approved",
                CreatedAt = new DateTime(2026, 2, 5)
            },

            new LoanApplication
            {
                Id = 3,
                UserId = "seed-user-2",
                LoanAmount = 2000,
                LoanPurpose = "Medical Expenses",
                DurationMonths = 6,
                InterestRate = 7.5m,
                Status = "Rejected",
                CreatedAt = new DateTime(2026, 3, 1)
            }

        );

            // Seed LoanTypes
            modelBuilder.Entity<LoanType>().HasData(
                new LoanType { Id = 1, Name = "Personal Loan", Description = "For personal expenses such as medical bills, vacations, or emergencies.", InterestRate = 12.0m, IsActive = true },
                new LoanType { Id = 2, Name = "Business Loan", Description = "For starting or expanding a business.", InterestRate = 8.0m, IsActive = true },
                new LoanType { Id = 3, Name = "Education Loan", Description = "For tuition fees, books, and other educational expenses.", InterestRate = 5.5m, IsActive = true },
                new LoanType { Id = 4, Name = "Mortgage Loan", Description = "For purchasing or refinancing a home.", InterestRate = 3.5m, IsActive = true },
                new LoanType { Id = 5, Name = "Auto Loan", Description = "For purchasing a new or used vehicle.", InterestRate = 6.5m, IsActive = true }
            );

            modelBuilder.Entity<LoanType>()
                .Property(l => l.InterestRate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<LoanApplication>()
                .HasOne(l => l.User)
                .WithMany(u => u.LoanApplications)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.LoanApplication)
                .WithOne(a => a.Loan)
                .HasForeignKey<Loan>(l => l.LoanApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
            // LoanApplication
            modelBuilder.Entity<LoanApplication>()
                .Property(l => l.LoanAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanApplication>()
                .Property(l => l.InterestRate)
                .HasPrecision(5, 2);

            // Loan
            modelBuilder.Entity<Loan>()
                .Property(l => l.PrincipalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Loan>()
                .Property(l => l.InterestRate)
                .HasPrecision(5, 2);

            // Repayment
            modelBuilder.Entity<Repayment>()
                .Property(r => r.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Repayment>()
                .Property(r => r.PrincipalPortion)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Repayment>()
                .Property(r => r.InterestPortion)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Repayment>()
                .HasOne(r => r.Loan)
                .WithMany(l => l.Repayments)
                .HasForeignKey(r => r.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Repayment>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // EligibilityCheck
            modelBuilder.Entity<EligibilityCheck>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EligibilityCheck>()
                .HasOne(e => e.LoanType)
                .WithMany()
                .HasForeignKey(e => e.LoanTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EligibilityCheck>()
                .Property(e => e.InterestRate)
                .HasPrecision(5, 2);

            // Bank seed data
            modelBuilder.Entity<Bank>().HasData(
                new Bank { Id = 1, Name = "Barclays", Code = "BARC", IsActive = true },
                new Bank { Id = 2, Name = "HSBC", Code = "HSBC", IsActive = true },
                new Bank { Id = 3, Name = "Lloyds Banking Group", Code = "LLOY", IsActive = true },
                new Bank { Id = 4, Name = "NatWest", Code = "NATW", IsActive = true },
                new Bank { Id = 5, Name = "Santander UK", Code = "SANT", IsActive = true }
            );

            // Seed SiteSettings
            modelBuilder.Entity<SiteSettings>().HasData(
                new SiteSettings
                {
                    Id = 1,
                    SiteName = "LoanApp",
                    Email = "support@loanapp.com",
                    Phone = "(800) 555-LOAN",
                    AddressLine1 = "123 Finance Street, Suite 400",
                    AddressLine2 = "New York, NY 10001",
                    BusinessHours = "Mon \u2013 Fri: 8AM \u2013 6PM EST",
                    FooterDescription = "Trusted lending solutions with competitive rates and transparent terms. We're committed to helping you achieve your financial goals with fast approvals and flexible repayment options.",
                    NmlsNumber = "123456",
                    UpdatedAt = new DateTime(2026, 1, 1)
                }
            );

            // LoanDisbursement relationships
            modelBuilder.Entity<LoanDisbursement>()
                .HasOne(d => d.LoanApplication)
                .WithMany()
                .HasForeignKey(d => d.LoanApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanDisbursement>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanDisbursement>()
                .HasOne(d => d.Bank)
                .WithMany()
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoanDisbursement>()
                .HasIndex(d => d.LoanApplicationId)
                .IsUnique();

            // LoanDisbursement precision
            modelBuilder.Entity<LoanDisbursement>()
                .Property(d => d.ApprovedAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LoanDisbursement>()
                .Property(d => d.PaidAmount)
                .HasPrecision(18, 2);

            // SupportTicket relationships
            modelBuilder.Entity<SupportTicket>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupportTicket>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketMessage>()
                .HasOne(m => m.SupportTicket)
                .WithMany(t => t.Messages)
                .HasForeignKey(m => m.SupportTicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketMessage>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
    
}