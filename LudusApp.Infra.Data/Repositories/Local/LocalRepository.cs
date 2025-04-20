using System.Data;
using LudusApp.Domain.Interfaces.Local;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data.Repositories.Local;

public class LocalRepository : RepositoryBase<Domain.Entities.Local.Local>, ILocalRepository
{
    private readonly LudusAppContext _context; // Reutilizando o contexto do EF
    private readonly IDbConnection _dbConnection; // Conexão do Dapper

    public LocalRepository(LudusAppContext context) : base(context)
    {
        _context = context;
        _dbConnection = context.Database.GetDbConnection(); // EF
    }
    protected override IQueryable<Domain.Entities.Local.Local> AdicionarIncludes(IQueryable<Domain.Entities.Local.Local> query)
    {
        return query
            .Include(l => l.Cidade)
            .Include(l => l.Empresa); 
    }

   
}