using MediatR;
using AutoMapper;
using CallCenterPlatform.Domain.Interfaces;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Queries;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public GetClientByIdQueryHandler(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id);
        if (client == null)
            throw new Exception($"Client with ID {request.Id} not found");

        return _mapper.Map<ClientDto>(client);
    }
}