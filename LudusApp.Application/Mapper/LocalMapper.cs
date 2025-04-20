using LudusApp.Application.Dtos.Local;
using LudusApp.Domain.Entities.Local;
using LudusApp.Domain.Entities.Localidades.Cidade;
using LudusApp.Domain.Empresas;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Mapper;

public class LocalMapper : IMapper<Local, LocalReadDto, LocalCreateDto, LocalUpdateDto>
{
    public Local MapToCreateEntity(LocalCreateDto dto)
    {
        return new Local
        {
            Nome = dto.Nome,
            Status = dto.Status,
            Cep = dto.Cep,
            Bairro = dto.Bairro,
            Rua = dto.Rua,
            DiasFuncionamento = MapDiasFuncionamento(dto.DiasFuncionamentoList),
            Complemento = dto.Complemento,
            HorarioAbertura = dto.HorarioAbertura,
            HorarioFechamento = dto.HorarioFechamento,
            Observacao = dto.Observacao,
            ValorHora = dto.ValorHora,
            EmpresaId = dto.EmpresaId,
            TenantId = dto.TenantId,
            CidadeId = dto.CidadeId
        };
    }

    public Local MapToUpdateEntity(LocalUpdateDto dto)
    {
        return new Local
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Status = dto.Status,
            Cep = dto.Cep,
            Bairro = dto.Bairro,
            Rua = dto.Rua,
            DiasFuncionamento = MapDiasFuncionamento(dto.DiasFuncionamentoList),
            Complemento = dto.Complemento,
            HorarioAbertura = dto.HorarioAbertura,
            HorarioFechamento = dto.HorarioFechamento,
            Observacao = dto.Observacao,
            ValorHora = dto.ValorHora,
            EmpresaId = dto.EmpresaId,
            TenantId = dto.TenantId,
            CidadeId = dto.CidadeId
        };
    }

    public void MapToExistingEntity(LocalUpdateDto dto, Local entity)
    {
        entity.Nome = dto.Nome;
        entity.Status = dto.Status;
        entity.Cep = dto.Cep;
        entity.Bairro = dto.Bairro;
        entity.Rua = dto.Rua;
        entity.DiasFuncionamento = MapDiasFuncionamento(dto.DiasFuncionamentoList);
        entity.Complemento = dto.Complemento;
        entity.HorarioAbertura = dto.HorarioAbertura;
        entity.HorarioFechamento = dto.HorarioFechamento;
        entity.Observacao = dto.Observacao;
        entity.ValorHora = dto.ValorHora;
        entity.EmpresaId = dto.EmpresaId;
        entity.TenantId = dto.TenantId;
        entity.CidadeId = dto.CidadeId;
    }

    public LocalReadDto MapToReadDto(Local local)
    {
        return new LocalReadDto
        {
            Id = local.Id,
            Nome = local.Nome,
            Status = local.Status,
            Cep = local.Cep,
            Bairro = local.Bairro,
            Rua = local.Rua,
            DiasFuncionamento = local.DiasFuncionamento,
            Complemento = local.Complemento,
            HorarioAbertura = local.HorarioAbertura,
            HorarioFechamento = local.HorarioFechamento,
            Observacao = local.Observacao,
            ValorHora = local.ValorHora,
            EmpresaId = local.EmpresaId,
            EmpresaNome = local.Empresa?.NomeFantasia,
            TenantId = local.TenantId,
            CidadeId = local.Cidade?.Id ?? 0,
            CidadeNome = local.Cidade?.Nome
        };
    }

    private DiasDaSemana MapDiasFuncionamento(List<string> diasList)
    {
        DiasDaSemana dias = 0;

        foreach (var dia in diasList)
        {
            if (Enum.TryParse(dia, out DiasDaSemana diaEnum) && diaEnum != DiasDaSemana.Nenhum)
            {
                dias |= diaEnum;
            }
        }

        return dias == 0 ? DiasDaSemana.Nenhum : dias;
    }
}
