using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class RepaymentRepository : Repository<Repayment>, IRepaymentRepository
    {
        private readonly ApplicationDbContext _db;

        public RepaymentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Repayment obj)
        {
            _db.Repayments.Update(obj);
        }
    }
}
