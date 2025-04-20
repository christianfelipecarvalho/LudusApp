using LudusApp.Application.DTOs;
using LudusApp.Domain.TemaSettings;

namespace LudusApp.Application.Mapper
{
    public class TemaMapper : IMapper<Tema, TemaReadDto, TemaReadDto, TemaReadDto>
    {
        public TemaReadDto MapToReadDto(Tema tema)
        {
            return new TemaReadDto
            {
                BorderRadius = tema.BorderRadius,
                DarkMode = tema.DarkMode,
                PrimaryColor = tema.PrimaryColor,
                SecondaryColor = tema.SecondaryColor,
                UsuarioId = tema.UsuarioId
            };
        }


        public Tema MapToCreateEntity(TemaReadDto dto)
        {
            return new Tema
            {
                BorderRadius = dto.BorderRadius,
                DarkMode = dto.DarkMode,
                PrimaryColor = dto.PrimaryColor,
                SecondaryColor = dto.SecondaryColor,
                UsuarioId = dto.UsuarioId
            };
        }

        public Tema MapToUpdateEntity(TemaReadDto dto)
        {
            return new Tema
            {
                BorderRadius = dto.BorderRadius,
                DarkMode = dto.DarkMode,
                PrimaryColor = dto.PrimaryColor,
                SecondaryColor = dto.SecondaryColor,
                UsuarioId = dto.UsuarioId
            };
        }

        void IMapper<Tema, TemaReadDto, TemaReadDto, TemaReadDto>.MapToExistingEntity(TemaReadDto dto, Tema entity)
        {
            entity.BorderRadius = dto.BorderRadius;
            entity.DarkMode = dto.DarkMode;
            entity.PrimaryColor = dto.PrimaryColor;
            entity.SecondaryColor = dto.SecondaryColor;
            entity.UsuarioId = dto.UsuarioId;
        }
    }
}