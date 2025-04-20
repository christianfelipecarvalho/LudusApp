using LudusApp.Application.DTOs;
using LudusApp.Application.Mapper;
using LudusApp.Domain.Interfaces.Base;
using LudusApp.Domain.Interfaces.TemaSettings;
using LudusApp.Domain.TemaSettings;

namespace LudusApp.Application.Services;

public class TemaService : BaseService<Tema, TemaReadDto, TemaReadDto, TemaReadDto>
{
    private readonly IRepositoryBase<Tema> _repository;
    private readonly ITemaRepository _temaRepository;
    private readonly IMapper<Tema,TemaReadDto, TemaReadDto, TemaReadDto> _temaMapper;

    public TemaService(IRepositoryBase<Tema> repository, ITemaRepository temaRepository, IMapper<Tema, TemaReadDto, TemaReadDto, TemaReadDto> temaMapper) : base(temaRepository, temaMapper)
    {
        _repository = repository;
        _temaRepository = temaRepository;
        _temaMapper = temaMapper;
    }   

    public async Task CriarUsuarioComTemaPadrao(string usuarioId)
    {
        var temaExistente = await _repository.GetSingleAsync(t => t.UsuarioId == usuarioId);

        if (temaExistente != null)
            return;

        var temaPadrao = new Tema
        {
            UsuarioId = usuarioId,
            PrimaryColor = "#FFFFFF",
            SecondaryColor = "#000000", 
            DarkMode = false, 
            BorderRadius = "8", 
            DataCadastro = DateTime.UtcNow,
            DataUltimaAlteracao = DateTime.UtcNow,
        };

        await _repository.AddAsync(temaPadrao);
    }

    public async Task<Tema> AtualizarOuCriarTemaAsync(Tema novoTema)
    {
        var temaExistente = await _repository.GetSingleAsync(t => t.UsuarioId == novoTema.UsuarioId);

        if (temaExistente != null)
        {
            // Atualiza as propriedades
            temaExistente.PrimaryColor = novoTema.PrimaryColor;
            temaExistente.SecondaryColor = novoTema.SecondaryColor;
            temaExistente.DarkMode = novoTema.DarkMode;
            temaExistente.BorderRadius = novoTema.BorderRadius;
            temaExistente.DataUltimaAlteracao = DateTime.UtcNow;

            return await _repository.UpdateAsync(temaExistente);
        }
        else
        {
            novoTema.DataCadastro = DateTime.UtcNow;
            novoTema.DataUltimaAlteracao = DateTime.UtcNow;

            await _repository.AddAsync(novoTema);
            return novoTema;
        }
    }

    public async Task<Tema> RecuperaPorIdUsuario(string idusuario)
    {
        var tema = await _temaRepository.RecuperaPorIdUsuario(idusuario);
        
        return tema;
    }
    
    

}