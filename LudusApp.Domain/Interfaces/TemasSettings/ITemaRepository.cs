using LudusApp.Domain.Interfaces.Base;
using LudusApp.Domain.TemaSettings;

namespace LudusApp.Domain.Interfaces.TemaSettings;

public interface ITemaRepository : IRepositoryBase<Tema>
{
    Task<Tema> RecuperaPorIdUsuario(string idusuario);
}