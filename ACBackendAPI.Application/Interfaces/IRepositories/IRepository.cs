
using System.Linq.Expressions;

namespace ACBackendAPI.Application.Interfaces.IRepositories
{

    public interface IAsyncRepository<T, TM> where T : class
    {
        Task<T> GetByIdAsync(TM id);
        Task<T> GetByAsync(ISpecification<T> spec);

        Task<T> GetByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeExpressions);

        Task<T> GetFirstAsync();
        Task<T> GetFirstAsync(params Expression<Func<T, object>>[] includeExpressions);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAllAsync(params Expression<Func<T, object>>[] includeExpressions);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[]? includeExpressions
        );
        Task<List<T>> ListAllFromStoredProcedureAsync(string sql, object[] parameters);

        Task<decimal> FindSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> select);

        Task<decimal> FindSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> select);

        //Task<T> AddAsync(T entity);
        void Add(T entity);

        Task AddAsync(T entity);
        Task AddListAsync(IEnumerable<T> entity);

        void UpdateListAsync(IEnumerable<T> entity);
        void Update(T entity);

        //Task DeleteAsync(T entity);
        void Delete(T entity);
        void DeleteList(IEnumerable<T> entities);
        Task<bool> SaveChangesAsync();
    }
}