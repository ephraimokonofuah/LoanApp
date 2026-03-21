using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        private readonly ApplicationDbContext _db;

        public BankRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Bank obj)
        {
            _db.Banks.Update(obj);
        }
    }
}
