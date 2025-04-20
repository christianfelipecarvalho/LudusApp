using LudusApp.Application.Dtos.Usuario;
using LudusApp.Domain.Usuarios;

namespace LudusApp.Application.Mapper;


public static class UsuarioMapper
{
    public static Usuario ToEntity(this CreateUsuarioDto dto)
    {
        return new Usuario
        {
            UserName = dto.UserName,
            Nome = dto.Nome,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento,
            Cpf = dto.Cpf,
            Cep = dto.Cep,
            Estado = dto.Estado,
            Endereco = dto.Endereco,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Telefone = dto.Telefone,
            Numero = dto.Numero
        };
    }
    public static void UpdateFromDto(this Usuario usuario, UpdateUsuarioDto dto)
    {
        usuario.UserName = dto.UserName;
        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Cpf = dto.Cpf;
        usuario.Cep = dto.Cep;
        usuario.Estado = dto.Estado;
        usuario.Endereco = dto.Endereco;
        usuario.Bairro = dto.Bairro;
        usuario.Cidade = dto.Cidade;
        usuario.Telefone = dto.Telefone;
        usuario.Numero = dto.Numero;
    }
    public static UpdateUsuarioDto ToUpdateDto(this Usuario usuario)
    {
        return new UpdateUsuarioDto
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            Nome = usuario.Nome,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento ?? DateTime.MinValue,
            Cpf = usuario.Cpf,
            Cep = usuario.Cep,
            Estado = usuario.Estado,
            Endereco = usuario.Endereco,
            Bairro = usuario.Bairro,
            Cidade = usuario.Cidade,
            Telefone = usuario.Telefone,
            Numero = usuario.Numero
        };
    }
    public static ReadUsuarioDto ToReadDto(this Usuario usuario)
    {
        return new ReadUsuarioDto
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            Nome = usuario.Nome,
            DataNascimento = usuario.DataNascimento ?? DateTime.MinValue, 
            Email = usuario.Email,
            Password = string.Empty, 
            Cpf = usuario?.Cpf,
            Cep = usuario?.Cep,
            Estado = usuario.Estado,
            Endereco = usuario.Endereco,
            Bairro = usuario.Bairro,
            Cidade = usuario.Cidade,
            Telefone = usuario.Telefone,
            Numero = usuario.Numero
        };
    }

   
    public static UpdateCampoUsuarioDto ToUpdateCampoDto(this Usuario usuario)
    {
        return new UpdateCampoUsuarioDto
        {
            UserName = usuario.UserName,
            Nome = usuario.Nome,
            Email = usuario.Email ?? string.Empty,
            Password = string.Empty, // Nunca retorna senha
            DataNascimento = usuario.DataNascimento ?? DateTime.MinValue,
            Cpf = usuario.Cpf,
            Cep = usuario.Cep,
            Estado = usuario.Estado,
            Endereco = usuario.Endereco,
            Bairro = usuario.Bairro,
            Cidade = usuario.Cidade,
            Telefone = usuario.Telefone,
            Numero = usuario.Numero
        };
    }
    public static void UpdateFromUpdateCampoDto(this Usuario usuario, UpdateCampoUsuarioDto dto)
    {
        usuario.UserName = dto.UserName;
        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Cpf = dto.Cpf;
        usuario.Cep = dto.Cep;
        usuario.Estado = dto.Estado;
        usuario.Endereco = dto.Endereco;
        usuario.Bairro = dto.Bairro;
        usuario.Cidade = dto.Cidade;
        usuario.Telefone = dto.Telefone;
        usuario.Numero = dto.Numero;
    }


}
