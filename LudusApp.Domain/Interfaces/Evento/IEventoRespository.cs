using LudusApp.Domain.Interfaces.Base;

namespace LudusApp.Domain.Interfaces.Evento;

public interface IEventoRespository : IRepositoryBase<Entities.Evento.Evento>
{
    Task<IEnumerable<Entities.Evento.Evento>> RecuperaEventoPeloLocal(Guid localId);
}