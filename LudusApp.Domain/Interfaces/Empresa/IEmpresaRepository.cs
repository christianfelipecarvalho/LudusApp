using LudusApp.Domain.Empresas; 
using LudusApp.Domain.Interfaces.Base; 

namespace LudusApp.Domain.Interfaces;

public interface IEmpresaRepository : IRepositoryBase<Empresa>
{
    Task<Empresa> GetByCnpjAsync(string cnpj);
    Task<Empresa> ObterEmpresaPorFiltrosAsync(string? cnpj, string? email, string? razaoSocial, Guid? tenantId);
}