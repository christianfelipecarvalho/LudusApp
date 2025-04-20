using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Dtos.Local
{
    public record LocalReadDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required]
        public EnumStatusLocal Status { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter 8 caracteres")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória")]
        public int CidadeId { get; set; }

        public string CidadeNome { get; set; }

        [Required(ErrorMessage = "Bairro é obrigatório")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Rua é obrigatória")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Dias de funcionamento são obrigatórios")]
        public DiasDaSemana DiasFuncionamento { get; set; }

        private List<string> _diasFuncionamentoList;
        public List<string> DiasFuncionamentoList 
        { 
            get 
            { 
                if (_diasFuncionamentoList == null)
                {
                    _diasFuncionamentoList = Enum.GetValues<DiasDaSemana>()
                        .Where(d => d != DiasDaSemana.Nenhum && DiasFuncionamento.HasFlag(d))
                        .Select(dia => dia.ToString())
                        .ToList();
                }
                return _diasFuncionamentoList;
            }
        }
        

        public string Complemento { get; set; }

        [Required(ErrorMessage = "Horário de abertura é obrigatório")]
        public TimeSpan HorarioAbertura { get; set; }

        [Required(ErrorMessage = "Horário de fechamento é obrigatório")]
        [Compare("HorarioAbertura", ErrorMessage = "Horário de fechamento não pode ser antes do horário de abertura.")]
        public TimeSpan HorarioFechamento { get; set; }

        public string Observacao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Valor hora deve ser positivo")]
        public double? ValorHora { get; set; }

        [Required]
        public Guid EmpresaId { get; set; }

        [Required(ErrorMessage = "Nome da empresa é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome da empresa não pode exceder 100 caracteres")]
        public string EmpresaNome { get; set; }

        [Required(ErrorMessage = "TenantId é obrigatório.")]
        public Guid TenantId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HorarioFechamento < HorarioAbertura)
            {
                yield return new ValidationResult(
                    "Horário de fechamento não pode ser antes do horário de abertura.",
                    new[] { nameof(HorarioFechamento) }
                );
            }
        }
    }
}
