using LudusApp.Domain.Entities.VinculosUsuarioEmpresa;
using LudusApp.Domain.Interfaces.Base;

namespace LudusApp.Domain.Interfaces.VinculoUsuarioEmpresa
{
    public interface IUsuarioEmpresaRepository : IRepositoryBase<UsuarioEmpresa>
    {
        Task<UsuarioEmpresa> AdicionarVinculoAsync(UsuarioEmpresa usuarioEmpresa);
    }
}
