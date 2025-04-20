    using LudusApp.Application.Dtos.Local;
    using LudusApp.Application.Mapper;
    using LudusApp.Domain.Entities.Local;
    using LudusApp.Domain.Interfaces.Local;

    namespace LudusApp.Application.Services;

    public class LocalService: BaseService<Local, LocalReadDto,LocalCreateDto,LocalUpdateDto>
    {
        private readonly ILocalRepository _localRepository;
        private readonly IMapper<Local, LocalReadDto,LocalCreateDto,LocalUpdateDto> _localMapper;

        public LocalService(ILocalRepository localRepository, IMapper<Local, LocalReadDto,LocalCreateDto,LocalUpdateDto> localMapper) : base(localRepository, localMapper)
        {
            _localRepository = localRepository;
            _localMapper = localMapper;
        }

        public async Task<List<LocalReadDto>> ObterLocaisComPaginacao(int pagina, int tamanhoPagina)
        {
            var locais = await _localRepository.RecuperaTodosComPaginacaoAsync(pagina, tamanhoPagina);
            return locais.Select(_localMapper.MapToReadDto).ToList();
        }

    }