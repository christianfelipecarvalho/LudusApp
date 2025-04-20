using System.Data;
using Dapper;
using LudusApp.Domain.Interfaces.Evento;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data.Repositories.Evento;

public class EventoRepository: RepositoryBase<Domain.Entities.Evento.Evento>, IEventoRespository
{
    private readonly LudusAppContext _context; // Reutilizando o contexto do EF
    private readonly IDbConnection _dbConnection; // Conexão do Dapper

    public EventoRepository(LudusAppContext context) : base(context)
    {
        _context = context;
        _dbConnection = context.Database.GetDbConnection(); // EF
    }
    protected override IQueryable<Domain.Entities.Evento.Evento> AdicionarIncludes(IQueryable<Domain.Entities.Evento.Evento> query)
    {
        return query
            .Include(l => l.Local)
            .Include(l => l.Usuario); 
    }
   


    public async Task<IEnumerable<Domain.Entities.Evento.Evento>> RecuperaEventoPeloLocal(Guid localId)
    {
        var sql = @"
        SELECT 
            ""Id"", ""Numero"", ""Nome"", ""Email"", ""Telefone"", 
            ""ValorTotal"", ""ValorHora"", ""HoraInicio"", ""HoraFim"", ""DataEvento"", ""Status"", 
            ""DataAlteracao"", ""IdLocal"", ""IdUsuario"", ""IdTenant"", ""Ativo"", 
            ""DataCadastro"", ""UsuarioCadastro"", ""DataUltimaAlteracao"", 
            ""UsuarioUltimaAlteracao"", ""RowVersion""
        FROM public.""Eventos""
        WHERE ""IdLocal"" = @LocalId;
    ";

        return await _dbConnection.QueryAsync<Domain.Entities.Evento.Evento>(sql, new { LocalId = localId });
    }
}