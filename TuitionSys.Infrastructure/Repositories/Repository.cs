    using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TuitionSys.Domain.Interfaces;
using TuitionSys.Infrastructure.Data;

namespace TuitionSys.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
    where T : class
    {
        public readonly QLHPSVDbContext _context;

        public readonly DbSet<T> _DbSet;
        public Repository(QLHPSVDbContext context)
        {
            _context = context;
            _DbSet = context.Set<T>();
        }
        public async Task<T> GetByIdAsync(string id) => await _DbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _DbSet.ToListAsync();
        public async Task AddAsync(T entity) => await _DbSet.AddAsync(entity);

        public void Update(T entity) => _DbSet.Update(entity);

        public void Delete(T entity) => _DbSet.Remove(entity);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
        {
            return await Task.FromResult(_DbSet.Where(predicate));
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _DbSet.RemoveRange(entities);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }


        public async Task InsertAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

    }

}
