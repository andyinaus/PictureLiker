using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PictureLiker.DAL.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(int id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> ListAsync();
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Edit(T entity);
        Task SaveAsync();
    }

    public abstract class EntityBase
    {
        [Key]
        public int Id { get; private set; }
    }
}
