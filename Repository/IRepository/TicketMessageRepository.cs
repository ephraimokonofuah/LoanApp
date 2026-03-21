using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class TicketMessageRepository : Repository<TicketMessage>, ITicketMessageRepository
    {
        private readonly ApplicationDbContext _db;

        public TicketMessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TicketMessage obj)
        {
            _db.TicketMessages.Update(obj);
        }
    }
}
