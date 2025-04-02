using LudusApp.Domain.Interfaces;
using LudusApp.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Application.Services;

public class BaseService<TEntity> where TEntity : class
{
    protected readonly IRepositoryBase<TEntity> _repository;

    public BaseService(IRepositoryBase<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<List<TEntity>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public async Task<TEntity> ObterPorIdIntAsync(int id)
    {
        return await _repository.ObterPorIdIntAsync(id);
    }
    public async Task<TEntity> ObterPorIdAsync(Guid id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<List<TEntity>> BuscarPorNomeAsync(string nome)
    {
        return await _repository.FindAsync(entity => EF.Functions.Like(EF.Property<string>(entity, "Nome"), $"%{nome}%"));
    }
}