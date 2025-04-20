using LudusApp.Application.Dtos.Localidades;
using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Entities.Localidades.Cidade;

namespace LudusApp.Application.Mappers;

public static class CidadeMapper
{
    // Converte de Entidade para DTO
    public static CidadeDto ToDto(Cidade cidade, string estadoNome)
    {
        return new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            EstadoId = cidade.EstadoId,
            EstadoNome = estadoNome // Recebe o nome do estado da consulta
        };
    }

    // Converte de DTO para Entidade
    public static Cidade ToEntity(CidadeDto cidadeDto)
    {
        return new Cidade
        {
            Id = cidadeDto.Id,
            Nome = cidadeDto.Nome,
            EstadoId = cidadeDto.EstadoId // EstadoId é mantido
        };
    }

}