using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class EligibilityCheckRepository : Repository<EligibilityCheck>, IEligibilityCheckRepository
    {
        private readonly ApplicationDbContext _db;

        public EligibilityCheckRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(EligibilityCheck obj)
        {
            _db.EligibilityChecks.Update(obj);
        }
    }
}
