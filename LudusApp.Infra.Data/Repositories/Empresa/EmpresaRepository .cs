using System.Data;
using Dapper;
using LudusApp.Domain.Empresas;
using LudusApp.Domain.Interfaces;
using LudusApp.Infra.Data;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infrastructure.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        private readonly LudusAppContext _context; // Reutilizando o contexto do EF
        private readonly IDbConnection _dbConnection; // Conexão do Dapper

        public EmpresaRepository(LudusAppContext context) : base(context)
        {
            _context = context;
            _dbConnection = context.Database.GetDbConnection(); // EF
        }

        // Método com EF
        public async Task<Empresa> GetByCnpjAsync(string cnpj)
        {
            return await _context.Empresas.FirstOrDefaultAsync(e => e.Cnpj == cnpj);
        }
        // DAPPER
        public async Task<Empresa> ObterEmpresaPorFiltrosAsync(string? cnpj, string? email, string? razaoSocial, Guid? tenantId)
        {
            string sql = @"
                SELECT *
                FROM public.""Empresas""
                WHERE ""Cnpj"" = @Cnpj OR ""Email"" = @Email OR ""RazaoSocial"" = @RazaoSocial OR ""TenantId"" = @TenantId";

            var empresa = await _dbConnection.QueryFirstOrDefaultAsync<Empresa>(sql, new
            {
                Cnpj = cnpj,
                Email = email,
                RazaoSocial = razaoSocial,
                TenantId = tenantId
            });

            return empresa;
        }

       
    }
}