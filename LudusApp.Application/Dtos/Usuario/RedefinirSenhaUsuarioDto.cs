namespace LudusApp.Application.Dtos.Usuario;

public class RedefinirSenhaUsuarioDto
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string NovaSenha { get; set; }
};