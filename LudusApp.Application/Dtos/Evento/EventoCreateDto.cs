using System.ComponentModel.DataAnnotations;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Dtos;

public record EventoCreateDto
{
    [Required(ErrorMessage = "Nome é obrigatório e deve ter entre 3 e 100 caracteres")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    public long? Numero { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Insira um email válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Valor total é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor total deve ser maior que zero")]
    public decimal ValorTotal { get; set; }

    [Required(ErrorMessage = "Valor hora é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor hora deve ser maior que zero")]
    public decimal ValorHora { get; set; }

    [Required(ErrorMessage = "Hora inicial é obrigatória")]
    public TimeSpan HoraInicio { get; set; }

    [Required(ErrorMessage = "Hora final é obrigatória")]
    public TimeSpan HoraFim { get; set; }

    [Required(ErrorMessage = "Data do evento é obrigatória")]
    public DateTime DataEvento { get; set; }

    [Required(ErrorMessage = "Local é obrigatório")]
    public Guid IdLocal { get; set; }

    public string? IdUsuario { get; set; }

    public Guid? IdTenant { get; set; }
}