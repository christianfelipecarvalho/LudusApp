using LudusApp.Application.Dtos.Empresa;
using LudusApp.Domain.Empresas;

namespace LudusApp.Application.Mapper
{
    public static class EmpresaMapper
    {
        /// <summary>
        /// Mapeia a entidade Empresa para o DTO ReadEmpresaDto.
        /// </summary>
        public static ReadEmpresaDto ToReadDto(this Empresa empresa)
        {
            return new ReadEmpresaDto
            {
                Id = empresa.Id,
                RazaoSocial = empresa.RazaoSocial,
                NomeFantasia = empresa.NomeFantasia,
                Apelido = empresa.Apelido,
                Email = empresa.Email,
                Telefone = empresa.Telefone,
                Ddd = empresa.Ddd,
                Endereco = empresa.Endereco,
                NumEndereco = empresa.NumEndereco,
                Cnpj = empresa.Cnpj,
                Im = empresa.Im,
                Ie = empresa.Ie,
                Estado = empresa.Estado,
                Cidade = empresa.Cidade,
                Bairro = empresa.Bairro,
                Cep = empresa.Cep,
                Status = empresa.Status,
                DataHoraCadastro = empresa.DataHoraCadastro,
                DataUltimaAlteracao = empresa.DataUltimaAlteracao,
                UsuarioCriacao = empresa.UsuarioCriacao,
                UsuarioUltimaAlteracao = empresa.UsuarioUltimaAlteracao,
                TenantId = empresa.TenantId
            };
        }

        /// <summary>
        /// Mapeia o DTO CreateEmpresaDto para a entidade Empresa.
        /// </summary>
        public static Empresa ToEntity(this CreateEmpresaDto dto)
        {
            return new Empresa
            {
                RazaoSocial = dto.RazaoSocial,
                NomeFantasia = dto.NomeFantasia,
                Apelido = dto.Apelido,
                Email = dto.Email,
                Cnpj = dto.Cnpj,
                Telefone = dto.Telefone,
                Ddd = dto.Ddd,
                Endereco = dto.Endereco,
                NumEndereco = dto.NumEndereco,
                Im = dto.Im,
                Ie = dto.Ie,
                Estado = dto.Estado,
                Cidade = dto.Cidade,
                Bairro = dto.Bairro,
                Cep = dto.Cep,
                Status = dto.Status,
                TenantId = dto.TenantId,
                UsuarioCriacao = dto.UsuarioCriacao,
                UsuarioUltimaAlteracao = dto.UsuarioCriacao,
                DataHoraCadastro = DateTime.UtcNow,
                DataUltimaAlteracao = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Atualiza a entidade Empresa com os valores do DTO UpdateEmpresaDto.
        /// </summary>
        public static void UpdateFromDto(this Empresa empresa, UpdateEmpresaDto dto)
        {
            empresa.RazaoSocial = dto.RazaoSocial;
            empresa.NomeFantasia = dto.NomeFantasia;
            empresa.Apelido = dto.Apelido;
            empresa.Email = dto.Email;
            empresa.Cnpj = dto.Cnpj;
            empresa.Telefone = dto.Telefone;
            empresa.Ddd = dto.Ddd;
            empresa.Endereco = dto.Endereco;
            empresa.NumEndereco = dto.NumEndereco;
            empresa.Im = dto.Im;
            empresa.Ie = dto.Ie;
            empresa.Estado = dto.Estado;
            empresa.Cidade = dto.Cidade;
            empresa.Bairro = dto.Bairro;
            empresa.Cep = dto.Cep;
            empresa.Status = dto.Status;
            empresa.UsuarioUltimaAlteracao = dto.UsuarioUltimaAlteracao;
            empresa.DataUltimaAlteracao = DateTime.UtcNow;
        }
    }
}