using System.ComponentModel;
using LudusApp.Application.Dtos;
using LudusApp.Domain.Entities.Evento;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Mapper;

 public class EventoMapper : IMapper<Evento, EventoReadDto, EventoCreateDto, EventoUpdateDto>
    {
        // Mapear de EventoCreateDto para Evento
        public Evento MapToCreateEntity(EventoCreateDto dto)
        {
            return new Evento
            {
                Id = Guid.NewGuid(), // Gerar um novo Id
                Nome = dto.Nome,
                Numero = dto.Numero,
                Email = dto.Email,
                Telefone = dto.Telefone,
                ValorTotal = dto.ValorTotal,
                ValorHora = dto.ValorHora,
                HoraInicio = dto.HoraInicio,
                HoraFim = dto.HoraFim,
                DataEvento = dto.DataEvento,
                DataAlteracao = DateTime.UtcNow, // Registrar a data da criação
                IdLocal = dto.IdLocal,
                IdUsuario = dto.IdUsuario,
                IdTenant = dto.IdTenant
            };
        }

        // Mapear de EventoUpdateDto para Evento
        public Evento MapToUpdateEntity(EventoUpdateDto dto)
        {
            return new Evento
            {
                Nome = dto.Nome,
                Numero = dto.Numero,

                Email = dto.Email,
                Telefone = dto.Telefone,
                ValorTotal = dto.ValorTotal,
                ValorHora = dto.ValorHora,
                HoraInicio = dto.HoraInicio,
                HoraFim = dto.HoraFim,
                DataEvento = dto.DataEvento,
                Status = dto.Status,
                DataAlteracao = DateTime.UtcNow, // Registrar a data da atualização
                IdLocal = dto.IdLocal,
                IdUsuario = dto.IdUsuario,
                IdTenant = dto.IdTenant
            };
        }

        // Mapear de EventoUpdateDto para Evento existente (para atualização)
        public void MapToExistingEntity(EventoUpdateDto dto, Evento entity)
        {
            entity.Nome = dto.Nome;
            entity.Email = dto.Email;
            entity.Telefone = dto.Telefone;
            entity.ValorTotal = dto.ValorTotal;
            entity.ValorHora = dto.ValorHora;
            entity.HoraInicio = dto.HoraInicio;
            entity.HoraFim = dto.HoraFim;
            entity.DataEvento = dto.DataEvento;
            entity.Status = dto.Status;
            entity.DataAlteracao = DateTime.UtcNow; // Registrar a data da alteração
            entity.IdLocal = dto.IdLocal;
            entity.IdUsuario = dto.IdUsuario;
            entity.IdTenant = dto.IdTenant;
        }

        // Mapear de Evento para EventoReadDto
        public EventoReadDto MapToReadDto(Evento entity)
        {
            return new EventoReadDto
            {
                Id = entity.Id,
                Numero = entity.Numero,
                Nome = entity.Nome,
                Email = entity.Email,
                Telefone = entity.Telefone,
                ValorTotal = entity.ValorTotal,
                ValorHora = entity.ValorHora,
                HoraInicio = entity.HoraInicio,
                HoraFim = entity.HoraFim,
                DataEvento = entity.DataEvento,
                Status = entity.Status,
                DataAlteracao = entity.DataAlteracao,
                IdLocal = entity.IdLocal,
                NomeLocal = entity.Local?.Nome, // Se precisar do nome do local
                IdUsuario = entity.IdUsuario,
                NomeUsuario = entity.Usuario?.Nome, // Se precisar do nome do usuário
                IdTenant = entity.IdTenant
            };
        }
  
        
    }