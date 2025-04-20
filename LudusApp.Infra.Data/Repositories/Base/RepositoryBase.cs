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

    // Implementação do RecuperaPorIdAsync usando Entity Framework
    public virtual async Task<T> RecuperaPorIdAsync(object id)
    {
        var keyProperty = _context.Model.FindEntityType(typeof(T))?
            .FindPrimaryKey()?
            .Properties
            .FirstOrDefault();

        if (keyProperty == null)
            throw new InvalidOperationException($"A entidade {typeof(T).Name} não possui chave primária definida.");

        var parameter = Expression.Parameter(typeof(T), "e");

        // Constrói a expressão: e => e.Id == id
        var property = Expression.Property(parameter, keyProperty.Name);
        var convertedId = Expression.Convert(Expression.Constant(id), property.Type);
        var equality = Expression.Equal(property, convertedId);
        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

        var query = AdicionarIncludes(_dbSet.AsQueryable());

        return await query.AsNoTracking().FirstOrDefaultAsync(lambda);
    }


    // Implementação do RecuperaPorIdIntAsync usando Entity Framework
    public virtual async Task<T> RecuperaPorIdIntAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Implementação do RecuperaTodosAsync usando Entity Framework
    public virtual async Task<List<T>> RecuperaTodosAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<List<T>> RecuperaTodosComPaginacaoAsync(int pagina, int tamanhoPagina)
    {
        var query = _dbSet.AsQueryable();

        query = AdicionarIncludes(query);

        return await query
            .AsNoTracking()
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();
    }
    protected virtual IQueryable<T> AdicionarIncludes(IQueryable<T> query)
    {
        return query;
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
    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(predicate);
    }


    // Implementação do DeleteAsync usando Entity Framework
    public async Task DeleteAsync(Guid id)
    {
        var entity = await RecuperaPorIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}