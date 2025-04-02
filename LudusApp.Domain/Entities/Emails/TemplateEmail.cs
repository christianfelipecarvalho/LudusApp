using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudusApp.Domain.Entities.Emails;

public class TemplateEmail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Tipo { get; set; } // Ex.: BoasVindas, ConfirmacaoEmail, RecuperacaoSenha
    public string Assunto { get; set; } // Ex.: Bem-vindo ao LudusApp!
    public string Mensagem { get; set; } // Corpo do email com placeholders como {Nome}, {Email}

    public TemplateEmail()
    {
        Tipo = "Generico"; // Valor padrão
    }
}