using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Interfaces;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data.Repositories;

public class LocalidadeRepository : RepositoryBase<Estado>, ILocalidadeRepository
{
    private readonly LudusAppContext _context;

    public LocalidadeRepository(LudusAppContext context) : base(context)
    {
        _context = context;
    }

    // Implementação: Buscar estados por nome
    public async Task<List<Estado>> BuscarEstadosPorNomeAsync(string nome)
    {
        return await _context.Estados
            .Where(e => EF.Functions.Like(e.Nome.ToLower(), $"%{nome.ToLower()}%"))
            .ToListAsync();
    }

    // Implementação: Paginação de cidades
    public async Task<List<Cidade>> ObterCidadesComPaginacaoAsync(int pagina, int tamanhoPagina)
    {
        if (pagina < 1) pagina = 1;

        if (tamanhoPagina < 1) tamanhoPagina = 10; // Tamanho padrão, caso o usuário passe um valor inválido

        return await _context.Cidades
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();
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
}