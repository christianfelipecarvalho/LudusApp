using LudusApp.Application.Dtos;
using LudusApp.Application.Mapper;
using LudusApp.Domain.Entities.Evento;
using LudusApp.Domain.Interfaces.Base;
using LudusApp.Domain.Interfaces.Evento;

namespace LudusApp.Application.Services;

public class EventoService : BaseService<Evento, EventoReadDto,EventoCreateDto,EventoUpdateDto>
{
    private readonly IEventoRespository _eventoRespository;
    private readonly IMapper<Evento, EventoReadDto,EventoCreateDto,EventoUpdateDto> _eventoMapper;
    public EventoService(IEventoRespository eventoRepository, IMapper<Evento, EventoReadDto, EventoCreateDto, EventoUpdateDto> mapper) : base(eventoRepository, mapper)
    {
        _eventoRespository = eventoRepository;
        _eventoMapper = mapper;
    }
    public async Task<List<EventoReadDto>> ObterEventoPorLocal(Guid localId)
    {
        var evento = await _eventoRespository.RecuperaEventoPeloLocal(localId);
       
        return evento.Select(_eventoMapper.MapToReadDto).ToList();
    }
    
}