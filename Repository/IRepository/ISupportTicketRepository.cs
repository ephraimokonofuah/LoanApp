using System.Linq.Expressions;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ISupportTicketRepository : IRepository<SupportTicket>
    {
        void Update(SupportTicket obj);
        new Task<bool> AnyAsync(Expression<Func<SupportTicket, bool>> filter);
    }
}
