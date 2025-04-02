using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudusApp.Domain.Entities.Emails;

public class ConfiguracaoEmail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string ServidorSMTP { get; set; } // Ex.: smtp.gmail.com
    public int Porta { get; set; } // Ex.: 587
    public string RemetenteEmail { get; set; } // Ex.: email@dominio.com
    public string Senha { get; set; } // Ex.: senha segura
    public bool EnableSSL { get; set; } // Ex.: true para conexões seguras

    public ConfiguracaoEmail()
    {
        EnableSSL = true; // Valor padrão
    }
}