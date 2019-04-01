using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PictureLiker.DAL.Entities;

namespace PictureLiker.DAL.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(int id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync();
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<IList<T>> ListAsync();
        Task<IList<T>> ListAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FromSql(string query, params object[] parameters);
        IEnumerable<T> TakeAtPage(int page, int numberOfRecordsPerPage, Expression<Func<T, bool>> predicate);
        void Add(T entity);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Edit(T entity);
        long LongCount(Expression<Func<T, bool>> predicate);
    }
}
