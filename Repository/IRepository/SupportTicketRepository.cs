using System.Linq.Expressions;
using LoanApp.Data;
using LoanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Repository.IRepository
{
    public class SupportTicketRepository : Repository<SupportTicket>, ISupportTicketRepository
    {
        private readonly ApplicationDbContext _db;

        public SupportTicketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SupportTicket obj)
        {
            _db.SupportTickets.Update(obj);
        }

        public new async Task<bool> AnyAsync(Expression<Func<SupportTicket, bool>> filter)
        {
            return await _db.SupportTickets.AnyAsync(filter);
        }
    }
}
