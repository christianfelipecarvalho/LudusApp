using LudusApp.Application.Mapper;
using LudusApp.Domain.Interfaces;
using LudusApp.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Application.Services;

public class BaseService<TEntity, TReadDto, TCreateDto, TUpdateDto> : IBaseService<TEntity, TReadDto, TCreateDto, TUpdateDto>
    where TEntity : class
{
    protected readonly IRepositoryBase<TEntity> _repository;
    protected readonly IMapper<TEntity, TReadDto, TCreateDto, TUpdateDto> _mapper;

    public BaseService(IRepositoryBase<TEntity> repository, IMapper<TEntity, TReadDto, TCreateDto, TUpdateDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TReadDto> AddAsync(TCreateDto dto)
    {
        var entidade = _mapper.MapToCreateEntity(dto);

        if (entidade.GetType().GetProperty("Nome") != null)
        {
            string nome = (string)entidade.GetType().GetProperty("Nome")!.GetValue(entidade)!;

            var existentes = await BuscarPorNomeAsync(nome);
            if (existentes.Any())
                throw new InvalidOperationException("Já existe uma entidade com esse nome.");
        }

        await _repository.AddAsync(entidade);
        return _mapper.MapToReadDto(entidade);
    }

    public async Task<List<TReadDto>> RecuperaTodosAsync()
    {
        var entidades = await _repository.RecuperaTodosAsync();
        return entidades.Select(_mapper.MapToReadDto).ToList();
    }
    
    public async Task<List<TReadDto>> RecuperaTodosComPaginacaoAsync(int pagina, int tamanhoPagina)
    {
        var entidades = await _repository.RecuperaTodosComPaginacaoAsync(pagina, tamanhoPagina);
        return entidades.Select(_mapper.MapToReadDto).ToList();
    }

    public async Task<TReadDto> RecuperaPorIdAsync(Guid id)
    {
        var entidade = await _repository.RecuperaPorIdAsync(id);
        return _mapper.MapToReadDto(entidade);
    }

    public async Task<TReadDto> RecuperaPorIdIntAsync(int id)
    {
        var entidade = await _repository.RecuperaPorIdIntAsync(id);
        return _mapper.MapToReadDto(entidade);
    }

    public async Task<List<TReadDto>> BuscarPorNomeAsync(string nome)
    {
        var entidades = await _repository.FindAsync(
            entity => EF.Functions.Like(EF.Property<string>(entity, "Nome"), $"%{nome}%")
        );
        return entidades.Select(_mapper.MapToReadDto).ToList();
    }

    public async Task<TReadDto> AtualizaAsync(Guid id, TUpdateDto dto)
    {
        var entidade = _mapper.MapToUpdateEntity(dto);
        var entidadeExistente = await _repository.RecuperaPorIdAsync(id);

        if (entidadeExistente == null)
            throw new InvalidOperationException("Entidade não encontrada para atualização.");

        _mapper.MapToExistingEntity(dto, entidadeExistente);
        await _repository.UpdateAsync(entidadeExistente);

        var entidadeAtualizada = _mapper.MapToReadDto(entidadeExistente);
        return entidadeAtualizada;
    }

    public async Task ExcluiAsync(Guid id)
    {
        var entidade = await _repository.RecuperaPorIdAsync(id);

        if (entidade == null)
            throw new InvalidOperationException("Entidade não encontrada para exclusão.");

        await _repository.DeleteAsync(id);
    }
}
