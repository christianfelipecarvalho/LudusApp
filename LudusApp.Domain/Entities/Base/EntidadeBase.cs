using System.ComponentModel.DataAnnotations;

namespace LudusApp.Domain.Entities;

public abstract class EntidadeBase
{
    public bool Ativo { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public long UsuarioCadastro { get; set; }
    public DateTime? DataUltimaAlteracao { get; set; }
    public long? UsuarioUltimaAlteracao { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}