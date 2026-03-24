using LoanApp.Data;

namespace LoanApp.Repository.IRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ILoanApplicationRepository LoanApplication { get; private set; }
        public IDocumentRepository Document { get; private set; }
        public ILoanTypeRepository LoanType { get; private set; }
        public IEligibilityCheckRepository EligibilityCheck { get; private set; }
        public IBankRepository Bank { get; private set; }
        public ILoanDisbursementRepository LoanDisbursement { get; private set; }
        public ISupportTicketRepository SupportTicket { get; private set; }
        public ITicketMessageRepository TicketMessage { get; private set; }
        public ILoanRepository Loan { get; private set; }
        public IRepaymentRepository Repayment { get; private set; }
        public ISiteSettingsRepository SiteSettings { get; private set; }
        public ICryptoWalletRepository CryptoWallet { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            LoanApplication = new LoanApplicationRepository(_db);
            Document = new DocumentRepository(_db);
            LoanType = new LoanTypeRepository(_db);
            EligibilityCheck = new EligibilityCheckRepository(_db);
            Bank = new BankRepository(_db);
            LoanDisbursement = new LoanDisbursementRepository(_db);
            SupportTicket = new SupportTicketRepository(_db);
            TicketMessage = new TicketMessageRepository(_db);
            Loan = new LoanRepository(_db);
            Repayment = new RepaymentRepository(_db);
            SiteSettings = new SiteSettingsRepository(_db);
            CryptoWallet = new CryptoWalletRepository(_db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
        
    }
}