using LudusApp.Application.Models.Read.Localidade;
using LudusApp.Application.Interfaces.ReadOnly.Base;

namespace LudusApp.Application.Interfaces.ReadOnly.Localidade;

public interface ILocalidadeReadRepository : IReadOnlyRepository<CidadeComEstadoNome>
{
    Task<IEnumerable<CidadeComEstadoNome>> ObterCidadesComEstadoPaginadoAsync(int pagina, int tamanhoPagina);
    Task<IEnumerable<CidadeComEstadoNome>> ObterCidadesPorEstadoIdAsync(int idEstdo,int pagina, int tamanhoPagina);
    Task<IEnumerable<CidadeComEstadoNome>> ObterCidadesPorEstadoPeloNomeAsync(string nomeEstado,int pagina, int tamanhoPagina);
}