using LudusApp.Application.Dtos.Localidades;
using LudusApp.Domain.Entities.Localidades;

namespace LudusApp.Application.Mappers;

public static class CidadeMapper
{
    // Converte de Entidade para DTO
    public static CidadeDto ToDto(Cidade cidade)
    {
        return new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            //EstadoId = cidade.EstadoId
        };
    }

    // Converte de DTO para Entidade
    public static Cidade ToEntity(CidadeDto cidadeDto)
    {
        return new Cidade
        {
            Id = cidadeDto.Id,
            Nome = cidadeDto.Nome,
            //EstadoId = cidadeDto.EstadoId
        };
    }
}