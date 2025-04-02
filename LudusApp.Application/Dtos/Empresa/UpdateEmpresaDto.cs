using LudusApp.Domain.Enums;

public class UpdateEmpresaDto
{
    public string RazaoSocial { get; set; }
    public string? NomeFantasia { get; set; }
    public string? Apelido { get; set; }
    public string Email { get; set; }
    public string Cnpj { get; set; }
    public string Telefone { get; set; }
    public string Ddd { get; set; }
    public string? Endereco { get; set; }
    public string? NumEndereco { get; set; }
    public int? Im { get; set; }
    public int? Ie { get; set; }
    public string? Estado { get; set; }
    public string? Cidade { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public EnumStatusEmpresa Status { get; set; }
    public string UsuarioUltimaAlteracao { get; set; } // Quem fez a alteração
}