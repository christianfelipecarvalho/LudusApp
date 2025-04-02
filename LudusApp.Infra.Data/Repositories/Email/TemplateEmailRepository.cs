using System.Data;
using LudusApp.Domain.Entities.Emails;
using LudusApp.Domain.Interfaces.Email;
using LudusApp.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;


namespace LudusApp.Infra.Data.Repositories.Email;

public class TemplateEmailRepository : RepositoryBase<TemplateEmail>, ITemplateEmailRepository
{
    private readonly LudusAppContext _context; // Reutilizando o contexto do EF
    private readonly IDbConnection _dbConnection; // Conexão do Dapper

    public TemplateEmailRepository(LudusAppContext context) : base(context)
    {
        _context = context;
        _dbConnection = context.Database.GetDbConnection(); // EF
    }
}
