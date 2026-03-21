using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class LoanTypeRepository : Repository<LoanType>, ILoanTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LoanTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LoanType obj)
        {
            _db.LoanTypes.Update(obj);
        }
    }
}
