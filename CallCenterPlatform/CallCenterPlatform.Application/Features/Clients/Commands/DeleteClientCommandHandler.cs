using MediatR;
using CallCenterPlatform.Domain.Interfaces;

namespace CallCenterPlatform.Application.Features.Clients.Commands;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, bool>
{
    private readonly IClientRepository _clientRepository;

    public DeleteClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<bool> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id);
        if (client == null)
            throw new Exception($"Client with ID {request.Id} not found");

        await _clientRepository.DeleteAsync(request.Id);
        return true;
    }
}

public class SoftDeleteClientCommandHandler : IRequestHandler<SoftDeleteClientCommand, bool>
{
    private readonly IClientRepository _clientRepository;

    public SoftDeleteClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<bool> Handle(SoftDeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id);
        if (client == null)
            throw new Exception($"Client with ID {request.Id} not found");

        await _clientRepository.SoftDeleteAsync(request.Id);
        return true;
    }
}