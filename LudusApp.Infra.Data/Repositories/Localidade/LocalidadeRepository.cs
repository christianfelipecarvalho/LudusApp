using System.Data;
using Dapper;
using LudusApp.Application.Dtos.Localidades;
using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Entities.Localidades.Cidade;
using LudusApp.Domain.Entities.Localidades.Estado;
using LudusApp.Domain.Interfaces;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data.Repositories;

public class LocalidadeRepository : RepositoryBase<Estado>, ILocalidadeRepository
{
    private readonly LudusAppContext _context;
    private readonly IDbConnection _dbConnection;

    public LocalidadeRepository(LudusAppContext context) : base(context)
    {
        _context = context;
        _dbConnection = _context.Database.GetDbConnection();
    }

    // Implementação: Buscar estados por nome
    public async Task<List<Estado>> BuscarEstadosPorNomeAsync(string nome)
    {
        return await _context.Estados
            .Where(e => EF.Functions.Like(e.Nome.ToLower(), $"%{nome.ToLower()}%"))
            .AsNoTracking()
            .ToListAsync();
    }

    // Implementação: Paginação de cidades
    public async Task<List<Cidade>> ObterCidadesComPaginacaoAsync(int pagina, int tamanhoPagina)
    {
        pagina = Math.Max(pagina, 1);
        tamanhoPagina = Math.Max(tamanhoPagina, 10);

        return await _context.Cidades
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .AsNoTracking()
                .ToListAsync()
            ;
    }


    // Implementação: Buscar cidades por nome
    public async Task<List<Cidade>> BuscarCidadesPorNomeAsync(string nome)
    {
        return await _context.Cidades
            .Where(e => EF.Functions.Like(e.Nome.ToLower(), $"{nome.ToLower()}%"))
            .ToListAsync();
    }

    public async Task<Cidade> ObterCidadesPorId(int id)
    {
        return await _context.Cidades.FindAsync(id);
    }

    public async Task<Estado> BuscarEstadoPorSiglaAsync(string uf)
    {
        return await _context.Estados.Where(e => e.Sigla.Equals(uf)).FirstOrDefaultAsync();
    }

    public async Task AddCidadeAsync(Cidade cidade)
    {
        await _context.Cidades.AddAsync(cidade);
    }

    public async Task UpdateCidadeAsync(Cidade cidade)
    {
        _context.Cidades.Update(cidade);
        await _context.SaveChangesAsync();
    }

}