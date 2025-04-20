namespace LudusApp.Application.Mapper;

public interface IMapper<TEntity, TReadDto, TCreateDto, TUpdateDto>
{
    TEntity MapToCreateEntity(TCreateDto dto);
    TEntity MapToUpdateEntity(TUpdateDto dto);
    void MapToExistingEntity(TUpdateDto dto, TEntity entity);
    TReadDto MapToReadDto(TEntity entity);
}