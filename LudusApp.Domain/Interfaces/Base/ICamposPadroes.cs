using System.ComponentModel.DataAnnotations;

public interface ICamposPadroes
{
    bool Ativo { get; set; }
    DateTime DataCadastro { get; set; }
    string UsuarioCadastro { get; set; }
    DateTime? DataUltimaAlteracao { get; set; }
    string? UsuarioUltimaAlteracao { get; set; }
    Guid? TenantId { get; set; }
    [Timestamp]
    byte[]? RowVersion { get; set; }
}