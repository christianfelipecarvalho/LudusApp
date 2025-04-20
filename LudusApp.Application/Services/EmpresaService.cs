using LudusApp.Application.Dtos.Empresa;
using LudusApp.Application.Mapper;
using LudusApp.Application.Services;
using LudusApp.Domain.Entities.VinculosUsuarioEmpresa;
using LudusApp.Domain.Enums;
using LudusApp.Domain.Interfaces;
using LudusApp.Domain.Interfaces.VinculoUsuarioEmpresa;
using LudusApp.Domain.Usuarios;

using Microsoft.AspNetCore.Identity;

public class EmpresaService
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly UserManager<Usuario> _userManager;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;
    private readonly UsuarioService _usuarioService;


    public EmpresaService(IEmpresaRepository empresaRepository, UserManager<Usuario> userManager, IUsuarioEmpresaRepository usuarioEmpresaRepository, UsuarioService usuarioService)
    {
        _empresaRepository = empresaRepository;
        _userManager = userManager;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
        _usuarioService = usuarioService;
    }

    public async Task<List<ReadEmpresaDto>> BuscarTodas()
    {
        var empresas = await _empresaRepository.RecuperaTodosAsync();
        return empresas.Select(e => e.ToReadDto()).ToList();
    }

    public async Task<ReadEmpresaDto> BuscarPorId(Guid id)
    {
        var empresa = await _empresaRepository.RecuperaPorIdAsync(id);
        if (empresa == null)
        {
            throw new ApplicationException("Empresa não encontrada.");
        }
        return empresa.ToReadDto();
    }

    public async Task<string> CadastrarOuVincularEmpresa(CreateEmpresaDto empresaDto)
    {
        // Valida os dados da empresa antes de qualquer ação
        await ValidarDadosEmpresaAsync(empresaDto);

        // Verifica se o usuário já existe no banco de dados
        var usuarioExistente = await _userManager.FindByEmailAsync(empresaDto.Email);

        string usuarioId = usuarioExistente == null ? await _usuarioService.CriarUsuarioComEmpresaDtoAsync(empresaDto) : usuarioExistente.Id;

        // Cria a empresa, agora que temos um usuário garantido
        var novaEmpresa = empresaDto.ToEntity();
        novaEmpresa.DataHoraCadastro = DateTime.UtcNow;
        novaEmpresa.UsuarioCriacao = empresaDto.Email;

        await _empresaRepository.AddAsync(novaEmpresa);

        // Vincula o usuário à empresa recém-criada
        var usuarioEmpresa = new UsuarioEmpresa
        {
            EmpresaId = novaEmpresa.Id,
            UsuarioId = usuarioId,
            Papel = EnumPapelUsuarioEmpresa.Admin, // Define o papel do usuário na empresa
            DataVinculo = DateTime.UtcNow,
            TenantId = novaEmpresa.TenantId
            
        };

        await _usuarioEmpresaRepository.AdicionarVinculoAsync(usuarioEmpresa);
        await _usuarioService.AdicionarTenantAoUsuarioAsync(usuarioId, empresaDto.TenantId);

        return usuarioExistente == null
            ? "Empresa cadastrada e usuário criado com sucesso!"
            : "Empresa cadastrada e vinculada ao usuário já existente!";
    }
    private async Task ValidarDadosEmpresaAsync(CreateEmpresaDto empresaDto)
    {
        var empresaExistente = await _empresaRepository.ObterEmpresaPorFiltrosAsync(empresaDto.Cnpj, empresaDto.Email, empresaDto.RazaoSocial, empresaDto.TenantId);

        if (empresaExistente != null)
        {
            if (empresaExistente.Cnpj == empresaDto.Cnpj)
            {
                throw new ApplicationException($"Já existe uma empresa cadastrada com o CNPJ: {empresaDto.Cnpj}");
            }

            if (empresaExistente.Email == empresaDto.Email)
            {
                throw new ApplicationException($"Já existe uma empresa cadastrada com o e-mail: {empresaDto.Email}");
            }

            if (empresaExistente.RazaoSocial == empresaDto.RazaoSocial)
            {
                throw new ApplicationException($"Já existe uma empresa cadastrada com a Razão Social: {empresaDto.RazaoSocial}");
            }
            if(empresaExistente.TenantId == empresaDto.TenantId)
            {
                throw new ApplicationException($"TenantId já utilizado: {empresaDto.TenantId}");
            }
        }
    }
    public async Task Atualizar(Guid id, UpdateEmpresaDto empresaDto)
    {
        var empresaExistente = await _empresaRepository.RecuperaPorIdAsync(id);
        if (empresaExistente == null)
        {
            throw new ApplicationException("Empresa não encontrada.");
        }

        empresaExistente.UpdateFromDto(empresaDto);
        await _empresaRepository.UpdateAsync(empresaExistente);
    }

    public async Task Deletar(Guid id)
    {
        await _empresaRepository.DeleteAsync(id);
    }
}