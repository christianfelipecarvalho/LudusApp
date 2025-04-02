using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LudusApp.Domain.Usuarios;

namespace LudusApp.Domain.TemaSettings;

public class Tema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string BorderRadius { get; set; }

    [Required]
    public bool DarkMode { get; set; }

    public string PrimaryColor { get; set; }

    public string SecondaryColor { get; set; }

    [Required]
    public string UsuarioId { get; set; } // Chave estrangeira para o usuário

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }
}