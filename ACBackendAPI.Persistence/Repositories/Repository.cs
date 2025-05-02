using ACBackendAPI.Application.Interfaces.IRepositories;
using ACBackendAPI.Persistence.Repositories.SpecificationEvaluator;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ACBackendAPI.Persistence.Repositories.Repository
{

    public class EfRepository<T, M> : IAsyncRepository<T, M>
    where T : class
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(M id)
        {
            return await _dbContext.Set<T>().FindAsync(id) ?? default!;
        }

        public virtual async Task<T> GetByAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync() ?? default!;
        }

        public virtual async Task<T> GetByAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeExpressions
        )
        {
            IQueryable<T> set = _dbContext.Set<T>();
            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }
            T result = await set.FirstOrDefaultAsync(predicate) ?? default!;
            return result;
        }

        public virtual async Task<T> GetFirstAsync()
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync() ?? default!;
        }

        public virtual async Task<T> GetFirstAsync(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = _dbContext.Set<T>();
            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }
            T result = await set.FirstOrDefaultAsync() ?? default!;
            return result;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = _dbContext.Set<T>();
            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }
            return await set.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[]? includeExpressions
        )
        {
            IQueryable<T> set = _dbContext.Set<T>();
            if (includeExpressions != null && includeExpressions.Count() > 0)
            {
                foreach (var includeExpression in includeExpressions)
                {
                    set = set.Include(includeExpression);
                }
            }

            IReadOnlyList<T> results = await set.AsNoTracking().Where(predicate).ToListAsync();
            return results;
        }

        public async Task<List<T>> ListAllFromStoredProcedureAsync(string sql, object[] parameters)
        {
            return await _dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        public async Task<decimal> FindSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> select)
        {
            var sum = await _dbContext.Set<T>().AsNoTracking().AnyAsync(predicate)
                ? await _dbContext.Set<T>().AsNoTracking().Where(predicate).Select(select).SumAsync()
                : 0;
            return sum;
        }

        /// <summary>
        /// Finds the sum.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="select">The select.</param>
        /// <returns>System.Decimal.</returns>
        public async Task<decimal> FindSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> select)
        {
            var sum = await _dbContext.Set<T>().AsNoTracking().Where(predicate).Select(select).DefaultIfEmpty(0).SumAsync();
            return sum;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddListAsync(IEnumerable<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void DeleteList(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void UpdateListAsync(IEnumerable<T> entity)
        {
            _dbContext.Set<T>().UpdateRange(entity);
        }
    }
}
