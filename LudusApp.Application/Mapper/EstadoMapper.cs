using LudusApp.Application.Dtos.Localidades;
using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Entities.Localidades.Estado;

namespace LudusApp.Application.Mappers;

public static class EstadoMapper
{
    // Converte de Entidade para DTO
    public static EstadoDto ToDto(Estado estado)
    {
        return new EstadoDto
        {
            Id = estado.Id,
            Nome = estado.Nome,
            Sigla = estado.Sigla
        };
    }

    // Converte de DTO para Entidade
    public static Estado ToEntity(EstadoDto estadoDto)
    {
        return new Estado
        {
            Id = estadoDto.Id,
            Nome = estadoDto.Nome,
            Sigla = estadoDto.Sigla
        };
    }
}