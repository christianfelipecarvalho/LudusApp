using System.Linq.Expressions;

namespace LudusApp.Domain.Interfaces.Base;

public interface IRepositoryBase<T> where T : class
{
    Task<T> RecuperaPorIdAsync(object id);
    Task<T> RecuperaPorIdIntAsync(int id);
    Task<List<T>> RecuperaTodosAsync();
    Task<List<T>> RecuperaTodosComPaginacaoAsync(int pagina, int tamanhoPagina);

    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> BuscarPorNomeAsync(Expression<Func<T, string>> propriedade, string nome);
    Task AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);

    Task DeleteAsync(Guid id);
}
