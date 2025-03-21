﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LudusApp.Domain.Usuarios;

public class Usuario : IdentityUser
{
    [Required(ErrorMessage = "O campo nome é obrigatório!")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "A data de nascimento é obrigatória!")]
    public DateTime? DataNascimento { get; set; }
    [Required(ErrorMessage = "O campo CPF é obrigatório!")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 000.000.000-00")]
    public string? Cpf { get; set; }

    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O CEP deve estar no formato 00000-000")]
    public string? Cep { get; set; }
    public string? GoogleId { get; set; }

    public bool ativo { get; set; }
    public string? Endereco { get; set; }
    public string? Bairro { get; set; }
    public string? Estado { get; set; }

    public string? Cidade { get; set; }
    public string? Telefone { get; set; }
    public string? Numero { get; set; }

    public Usuario() : base() { }
}
