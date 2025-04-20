using LudusApp.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LudusApp.Domain.Entities.Emails;

public class Email
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Destinatario { get; set; }
    public string Assunto { get; set; }
    public string Mensagem { get; set; }
    public EnumStatusEmail Status { get; set; }
    public DateTime DataEnvio { get; set; }
    public DateTime? DataErro { get; set; }

    public Email()
    {
        Id = Guid.NewGuid();
        Status = EnumStatusEmail.NAOENVIADO; 
        DataEnvio = DateTime.MinValue;
    }
}