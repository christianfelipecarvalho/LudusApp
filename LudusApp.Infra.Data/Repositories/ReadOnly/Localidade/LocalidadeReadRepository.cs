using Dapper;
using LudusApp.Application.Interfaces.ReadOnly.Localidade;
using LudusApp.Application.Models.Read.Localidade;
using System.Data;

namespace LudusApp.Infra.Data.Repositories.ReadOnly.Localidade;

public class LocalidadeReadRepository : ILocalidadeReadRepository
{
    private readonly IDbConnection _dbConnection;

    public LocalidadeReadRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CidadeComEstadoNome> RecuperaPorIdTipoInt(int id)
    {
        var sql = @"SELECT c.""Id"", c.""Nome"", c.""EstadoId"", e.""Nome"" AS ""EstadoNome""
                    FROM public.""Cidades"" c
                    INNER JOIN public.""Estados"" e ON c.""EstadoId"" = e.""Id""
                    WHERE c.""Id"" = @Id";

        return await _dbConnection.QueryFirstOrDefaultAsync<CidadeComEstadoNome>(sql, new { Id = id });
    }

    public async Task<IEnumerable<CidadeComEstadoNome>> RecuperarTodos()
    {
        var sql = @"SELECT c.""Id"", c.""Nome"", c.""EstadoId"", e.""Nome"" AS ""EstadoNome""
                    FROM public.""Cidades"" c
                    INNER JOIN public.""Estados"" e ON c.""EstadoId"" = e.""Id""";

        return await _dbConnection.QueryAsync<CidadeComEstadoNome>(sql);
    }

    public async Task<IEnumerable<CidadeComEstadoNome>> ObterCidadesComEstadoPaginadoAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var sql = @"SELECT c.""Id"", c.""Nome"", c.""EstadoId"", e.""Nome"" AS ""EstadoNome""
                    FROM public.""Cidades"" c
                    INNER JOIN public.""Estados"" e ON c.""EstadoId"" = e.""Id""
                    ORDER BY c.""Nome""
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        return await _dbConnection.QueryAsync<CidadeComEstadoNome>(sql, new
        {
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
    }

    public async Task<IEnumerable<CidadeComEstadoNome>> ObterCidadesPorEstadoIdAsync(int idEstado, int pagina, int tamanhoPagina)
    {
        pagina = Math.Max(pagina, 1);
        tamanhoPagina = Math.Max(tamanhoPagina, 10);

        var sql = @"SELECT c.""Id"", c.""Nome"", c.""EstadoId"", e.""Nome"" AS ""EstadoNome""
                FROM public.""Cidades"" c
                INNER JOIN public.""Estados"" e ON c.""EstadoId"" = e.""Id""
                WHERE e.""Id"" = @EstadoId
                ORDER BY c.""Nome""
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        return await _dbConnection.QueryAsync<CidadeComEstadoNome>(sql, new
        {
            EstadoId = idEstado,
            Offset = (pagina - 1) * tamanhoPagina,
            PageSize = tamanhoPagina
        });
    }


    public async Task<IEnumerable<CidadeComEstadoNome>> ObterCidadesPorEstadoPeloNomeAsync(string nomeEstado, int pagina, int tamanhoPagina)
    {
        pagina = Math.Max(pagina, 1);
        tamanhoPagina = Math.Max(tamanhoPagina, 10);

        var sql = @"SELECT c.""Id"", c.""Nome"", c.""EstadoId"", e.""Nome"" AS ""EstadoNome""
                FROM public.""Cidades"" c
                INNER JOIN public.""Estados"" e ON c.""EstadoId"" = e.""Id""
                WHERE LOWER(e.""Nome"") LIKE LOWER(@NomeEstado)
                ORDER BY c.""Nome""
                OFFSET @Offset LIMIT @PageSize";

        return await _dbConnection.QueryAsync<CidadeComEstadoNome>(sql, new
        {
            NomeEstado = $"%{nomeEstado}%",
            Offset = (pagina - 1) * tamanhoPagina,
            PageSize = tamanhoPagina
        });
    }

}
