using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Interfaces.Base;

namespace LudusApp.Domain.Interfaces;

public interface ILocalidadeRepository : IRepositoryBase<Estado>
{
    /// <summary>
    /// Métodos específicos para Estados
    /// </summary>
    /// <param name="Busca pelo nome"></param>
    /// <returns></returns>
    Task<List<Estado>> BuscarEstadosPorNomeAsync(string nome);

    /// <summary>
    /// Busca cidade por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Cidade> ObterCidadesPorId(int id);


    /// <summary>
    /// Métodos específicos para Cidades
    /// </summary>
    /// <param name="pagina"></param>
    /// <param name="tamanhoPagina"></param>
    /// <returns></returns>
    Task<List<Cidade>> ObterCidadesComPaginacaoAsync(int pagina, int tamanhoPagina);
    /// <summary>
    /// Metodo especifico para cidade
    /// </summary>
    /// <param name="nome"></param>
    /// <returns></returns>
    Task<List<Cidade>> BuscarCidadesPorNomeAsync(string nome);
    Task<Estado> BuscarEstadoPorSiglaAsync(string uf);
}