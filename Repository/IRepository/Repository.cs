using System;
using System.Linq.Expressions;
using LoanApp.Data;
using LoanApp.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace LoanApp.Repository.IRepository
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();

        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;    
            }
            else
            {
                query = dbSet.AsNoTracking();  
            }

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }
      

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim()))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            return await query.AnyAsync(filter);
        }

    }

    public class T
    {
    }
}
