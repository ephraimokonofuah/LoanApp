using System.Linq.Expressions;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ITicketMessageRepository : IRepository<TicketMessage>
    {
        void Update(TicketMessage obj);
    }
}
