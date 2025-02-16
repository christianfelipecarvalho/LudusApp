using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LudusApp.Domain.Usuarios;
using Microsoft.IdentityModel.Tokens;

namespace LudusApp.Application.Services;

public class TokenService
{
    public string GenerateToken(Usuario usuario)
    {

        Claim[] claims = new Claim[]
        {

            new Claim("username", usuario.UserName),
            new Claim("id", usuario.Id),
            new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString())
        };
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6A4FSD568DS1F652VDV1FD656ED4FDds456ds13"));
        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(expires: DateTime.Now.AddMinutes(24), claims: claims, signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}