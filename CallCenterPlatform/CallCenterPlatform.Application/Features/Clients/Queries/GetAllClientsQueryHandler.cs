using MediatR;
using AutoMapper;
using CallCenterPlatform.Domain.Interfaces;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Queries;

public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, IEnumerable<ClientDto>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public GetAllClientsQueryHandler(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = request.IncludeInactive 
            ? await _clientRepository.GetAllAsync() 
            : await _clientRepository.GetActiveClientsAsync();
            
        return _mapper.Map<IEnumerable<ClientDto>>(clients);
    }
}