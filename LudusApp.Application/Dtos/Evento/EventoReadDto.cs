using System.ComponentModel;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Dtos;

public record EventoReadDto
{
    public Guid Id { get; set; }
    public long? Numero { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal ValorHora { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim { get; set; }
    public DateTime DataEvento { get; set; }

    public EnumStatusEvento Status { get; set; }

    // A propriedade para retornar a descrição do status
    public string StatusDescricao =>  EnumHelper.GetEnumDescription(Status);

    public DateTime DataAlteracao { get; set; }
    public Guid IdLocal { get; set; }
    public string? NomeLocal { get; set; }  
    public string? IdUsuario { get; set; }
    public string? NomeUsuario { get; set; }  
    public Guid? IdTenant { get; set; }


}