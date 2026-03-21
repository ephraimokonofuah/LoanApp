using System.Linq.Expressions;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface IDocumentRepository : IRepository<Document>
    {
        IEnumerable<Document> GetDocumentsByApplicationId(int applicationId);
        void Update(Document obj);
    }
}
