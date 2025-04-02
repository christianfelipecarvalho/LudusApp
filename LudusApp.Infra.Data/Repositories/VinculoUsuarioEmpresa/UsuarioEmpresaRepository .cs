using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusApp.Domain.Empresas;
using LudusApp.Domain.Interfaces;
using LudusApp.Infra.Data.Repositories.Base;
using LudusApp.Infra.Data;
using LudusApp.Domain.Entities.VinculosUsuarioEmpresa;
using LudusApp.Domain.Interfaces.VinculoUsuarioEmpresa;

namespace LudusApp.Infrastructure.Repositories
{
    public class UsuarioEmpresaRespository : RepositoryBase<UsuarioEmpresa>, IUsuarioEmpresaRepository
    {
        public UsuarioEmpresaRespository(LudusAppContext context) : base(context) { }

        public async Task<UsuarioEmpresa> AdicionarVinculoAsync(UsuarioEmpresa usuarioEmpresa)
        {
            // Adiciona o vínculo no contexto
            await _context.Set<UsuarioEmpresa>().AddAsync(usuarioEmpresa);
            // Salva as alterações no banco
            await _context.SaveChangesAsync();

            // Retorna o objeto recém-adicionado
            return usuarioEmpresa;
        }

    }
}