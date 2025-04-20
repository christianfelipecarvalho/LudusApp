using System.Data;
using Dapper;
using LudusApp.Domain.Interfaces.TemaSettings;
using LudusApp.Domain.TemaSettings;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data.Repositories.TemasSettings
{
    public class TemaRepository : RepositoryBase<Tema>, ITemaRepository
    {
        private readonly LudusAppContext _context; // Reutilizando o contexto do EF
        private readonly IDbConnection _dbConnection; // Conexão do Dapper

        public TemaRepository(LudusAppContext context) : base(context)
        {
            _context = context;
            _dbConnection = context.Database.GetDbConnection(); // EF
        }

        public override async Task<Tema> RecuperaPorIdAsync(object id)
        {
            string sql = "SELECT * FROM public.\"Tema\" WHERE \"Cnpj\" = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Tema>(sql, new { Id = id });
        }


        public async Task<Tema> RecuperaPorIdUsuario(string id)
        {
            string sql = "SELECT * FROM public.\"Temas\" WHERE \"UsuarioId\" = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Tema>(sql, new { Id = id });
        }
    }
}