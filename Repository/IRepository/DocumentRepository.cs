using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        private readonly ApplicationDbContext _db;

        public DocumentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Document> GetDocumentsByApplicationId(int applicationId)
        {
            return _db.Documents.Where(d => d.LoanApplicationId == applicationId).ToList();
        }

        public void Update(Document obj)
        {
            _db.Documents.Update(obj);
        }
    }
}
