namespace LoanApp.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ILoanApplicationRepository LoanApplication { get;}
        IDocumentRepository Document { get;}
        ILoanTypeRepository LoanType { get;}
        IEligibilityCheckRepository EligibilityCheck { get;}
        IBankRepository Bank { get;}
        ILoanDisbursementRepository LoanDisbursement { get;}
        ISupportTicketRepository SupportTicket { get;}
        ITicketMessageRepository TicketMessage { get;}
        void Save();
    }
}