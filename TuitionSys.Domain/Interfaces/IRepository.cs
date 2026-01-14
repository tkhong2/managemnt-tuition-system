using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TuitionSys.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        void RemoveRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task InsertAsync(T entity);

    }
}
