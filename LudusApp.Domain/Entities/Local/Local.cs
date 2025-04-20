using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LudusApp.Domain.Empresas;
using LudusApp.Domain.Entities.Localidades.Cidade;
using LudusApp.Domain.Entities.Localidades.Estado;
using LudusApp.Domain.Enums;

namespace LudusApp.Domain.Entities.Local;

public class Local : EntidadeBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório e deve ter entre 3 e 100 caracteres")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
    public string Nome { get; set; }

    [Required]
    public EnumStatusLocal Status { get; set; }

    [Required(ErrorMessage = "CEP é obrigatório")]
    public string Cep { get; set; }

    [Required(ErrorMessage = "Cidade é obrigatória")]
    public int CidadeId { get; set; }

    [ForeignKey("CidadeId")]
    public Cidade Cidade { get; set; }

    [Required(ErrorMessage = "Bairro é obrigatório")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "Rua é obrigatório")]
    public string Rua { get; set; }

    [Required(ErrorMessage = "Dias de funcionamento é obrigatório")]
    public DiasDaSemana DiasFuncionamento { get; set; }


    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Horário de abertura é obrigatório")]
    public TimeSpan HorarioAbertura { get; set; }

    [Required(ErrorMessage = "Horário de fechamento é obrigatório")]
    public TimeSpan HorarioFechamento { get; set; }

    public string? Observacao { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Valor hora deve ser positivo")]
    public double? ValorHora { get; set; }

    [Required]
    public Guid EmpresaId { get; set; }

    [ForeignKey("EmpresaId")] 
    public Empresa Empresa { get; set; } = null!;
    
    [Required(ErrorMessage = "TenantId é obrigatório.")]
    [Column("TenantId")]
    public Guid TenantId { get; set; } 
    public virtual ICollection<Evento.Evento> Eventos { get; set; } = new List<Evento.Evento>();


    // public List<ImagemLocal> Imagens { get; set; } = new();

    [NotMapped]
    public List<DayOfWeek> DiasFuncionamentoList
    {
        get
        {
            return Enum.GetValues(typeof(DiasDaSemana))
                .Cast<DiasDaSemana>()
                .Where(d => d != DiasDaSemana.Nenhum && DiasFuncionamento.HasFlag(d))
                .Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d.ToString()))
                .ToList();
        }
    }


}
