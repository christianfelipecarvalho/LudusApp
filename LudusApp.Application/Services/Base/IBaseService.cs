using System.Linq.Expressions;

namespace LudusApp.Application.Services;

public interface IBaseService<TEntity, TReadDto, TCreateDto, TUpdateDto>
{
    Task<TReadDto> AddAsync(TCreateDto  dto);
    Task<List<TReadDto>> RecuperaTodosAsync();
    Task<TReadDto> RecuperaPorIdAsync(Guid id);
    Task<TReadDto> RecuperaPorIdIntAsync(int id);
    Task<List<TReadDto>> BuscarPorNomeAsync(string nome);
    Task<TReadDto>  AtualizaAsync(Guid id, TUpdateDto dto);
    Task ExcluiAsync(Guid id);
}
