using LudusApp.Domain.Interfaces.Base;
using LudusApp.Domain.TemaSettings;

namespace LudusApp.Application.Services;

public class TemaService : BaseService<Tema>
{
    public TemaService(IRepositoryBase<Tema> repository) : base(repository)
    {
    }
}