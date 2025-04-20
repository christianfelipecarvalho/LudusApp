namespace LudusApp.Application.Interfaces.ReadOnly.Base;

public interface IReadOnlyRepository<T>
{
    Task<T> RecuperaPorIdTipoInt(int id);
    Task<IEnumerable<T>> RecuperarTodos();
}