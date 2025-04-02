using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Dtos.Empresa;

public class CreateEmpresaDto
{
    [Required]
    public string RazaoSocial { get; set; }
    public string NomeFantasia { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string? Apelido { get; set; }
    public string Cnpj { get; set; }
    public string Telefone { get; set; }
    public string Ddd { get; set; }

    public string Endereco { get; set; }

    public string NumEndereco { get; set; }
    public int? Im { get; set; } // Inscrição Municipal (se necessário)

    public int? Ie { get; set; } // Inscrição Estadual (se necessário)

    public string Estado { get; set; }

    public string Cidade { get; set; }

    public string Bairro { get; set; }

    public string Cep { get; set; }

    public EnumStatusEmpresa Status { get; set; }
    public string? NomeUsuario { get; set; }

    public string? SenhaTemporaria { get; set; }
    public DateTime? DataNascimentoUsuario { get; set; }
    public string? cpfUsuario { get; set; }

    public bool Padrao { get; set; }

    public DateTime DataHoraCadastro { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }
    public string UsuarioCriacao { get; set; } // Nome do usuário que criou a empresa
    public string UsuarioUltimaAlteracao { get; set; }
    [Column("TenantId")]
    public Guid? TenantId { get; set; } // Suporte ao TenantId (opcional)


}
