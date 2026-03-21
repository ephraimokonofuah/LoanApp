using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class LoanDisbursementRepository : Repository<LoanDisbursement>, ILoanDisbursementRepository
    {
        private readonly ApplicationDbContext _db;

        public LoanDisbursementRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LoanDisbursement obj)
        {
            _db.LoanDisbursements.Update(obj);
        }
    }
}
