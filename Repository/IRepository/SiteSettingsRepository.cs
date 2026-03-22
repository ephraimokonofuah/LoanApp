using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class SiteSettingsRepository : Repository<SiteSettings>, ISiteSettingsRepository
    {
        private readonly ApplicationDbContext _db;

        public SiteSettingsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SiteSettings obj)
        {
            _db.SiteSettings.Update(obj);
        }
    }
}
