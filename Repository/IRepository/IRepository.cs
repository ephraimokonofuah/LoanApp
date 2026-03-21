using System;
using System.Collections;
using System.Linq.Expressions;

namespace LoanApp.Repository.IRepository
{

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);

        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);


    }

}