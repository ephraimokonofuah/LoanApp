using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ISiteSettingsRepository : IRepository<SiteSettings>
    {
        void Update(SiteSettings obj);
    }
}
