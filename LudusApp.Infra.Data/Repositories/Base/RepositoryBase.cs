using System.Linq.Expressions;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using LudusApp.Domain.Interfaces.Base;

namespace LudusApp.Infra.Data.Repositories.Base;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly LudusAppContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly IDbConnection _dbConnection;

    public RepositoryBase(LudusAppContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _dbConnection = _context.Database.GetDbConnection();
    }

    // Implementação do ObterPorIdAsync usando Entity Framework
    public virtual async Task<T> ObterPorIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Implementação do ObterPorIdIntAsync usando Entity Framework
    public virtual async Task<T> ObterPorIdIntAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Implementação do ObterTodosAsync usando Entity Framework
    public virtual async Task<List<T>> ObterTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Implementação do FindAsync usando Entity Framework
    public  async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    // Implementação do BuscarPorNomeAsync usando Entity Framework
    public async Task<List<T>> BuscarPorNomeAsync(Expression<Func<T, string>> propriedade, string nome)
    {
        var propriedadeNome = ((MemberExpression)propriedade.Body).Member.Name; // Extrair o nome da propriedade

        return await _dbSet
            .Where(entity => EF.Functions.Like(EF.Property<string>(entity, propriedadeNome), $"%{nome}%"))
            .ToListAsync();
    }

    // Implementação do AddAsync usando Entity Framework
    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // Implementação do UpdateAsync usando Entity Framework
    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    // Implementação do DeleteAsync usando Entity Framework
    public async Task DeleteAsync(Guid id)
    {
        var entity = await ObterPorIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}