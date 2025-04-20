using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LudusApp.Domain.Entities;
using LudusApp.Domain.Entities.VinculosUsuarioEmpresa;
using LudusApp.Domain.Enums;

namespace LudusApp.Domain.Empresas;

public class Empresa : EntidadeBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string RazaoSocial { get; set; }

    public string? NomeFantasia { get; set; }

    public string? Apelido { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Telefone { get; set; }

    public string Ddd { get; set; }

    public string Endereco { get; set; }

    public string NumEndereco { get; set; }

    [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", ErrorMessage = "O CNPJ deve estar no formato 00.000.000/0000-00")]
    public string Cnpj { get; set; }

    public int? Im { get; set; } // Inscrição Municipal (se necessário)

    public int? Ie { get; set; } // Inscrição Estadual (se necessário)

    public string Estado { get; set; }

    public string Cidade { get; set; }

    public string Bairro { get; set; }

    public string Cep { get; set; }

    public EnumStatusEmpresa Status { get; set; }


    public DateTime DataHoraCadastro { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }
    public string UsuarioCriacao { get; set; } // Nome do usuário que criou a empresa
    public string UsuarioUltimaAlteracao { get; set; }

    [Required(ErrorMessage = "TenantId é obrigatório.")]
    [Column("TenantId")]
    public Guid TenantId { get; set; } 


    // Relação com Usuario
    //public ICollection<Usuario> Usuarios { get; set; }
    public ICollection<UsuarioEmpresa> UsuariosEmpresas { get; set; }

}


