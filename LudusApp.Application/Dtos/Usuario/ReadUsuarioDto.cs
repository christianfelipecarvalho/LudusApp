using System.ComponentModel.DataAnnotations;

namespace LudusApp.Application.Dtos.Usuario;

public class ReadUsuarioDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Cpf { get; set; }
    public string Cep { get; set; }
    public string? Estado { get; set; }

    public string Endereco { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Telefone { get; set; }
    public string Numero { get; set; }

}
