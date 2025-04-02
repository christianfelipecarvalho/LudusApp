using System.Linq.Expressions;

namespace LudusApp.Domain.Interfaces.Base;

public interface IRepositoryBase<T> where T : class
{
    Task<T> ObterPorIdAsync(Guid id);
    Task<T> ObterPorIdIntAsync(int id);
    Task<List<T>> ObterTodosAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> BuscarPorNomeAsync(Expression<Func<T, string>> propriedade, string nome);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
