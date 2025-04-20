using System.ComponentModel.DataAnnotations;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Dtos.Local;

public class LocalUpdateDto
{
    [Required(ErrorMessage = "Campo id é obrigatorio")]

    public Guid Id { get; set; }
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; }

    [Required]
    public EnumStatusLocal Status { get; set; }

    [Required(ErrorMessage = "CEP é obrigatório")]
    [StringLength(8, MinimumLength = 8)]
    public string Cep { get; set; }

    [Required]
    public int CidadeId { get; set; }

    [Required]
    public string Bairro { get; set; }

    [Required]
    public string Rua { get; set; }

    [Required(ErrorMessage = "Informe os dias de funcionamento")]
    public List<string> DiasFuncionamentoList { get; set; } = new();


    public string Complemento { get; set; }

    [Required]
    public TimeSpan HorarioAbertura { get; set; }

    [Required]
    public TimeSpan HorarioFechamento { get; set; }

    public string Observacao { get; set; }

    [Range(0, double.MaxValue)]
    public double? ValorHora { get; set; }

    [Required]
    public Guid EmpresaId { get; set; }

    [Required]
    public Guid TenantId { get; set; }
}