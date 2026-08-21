using MediatR;
using AutoMapper;
using CallCenterPlatform.Domain.Entities;
using CallCenterPlatform.Domain.Enums;
using CallCenterPlatform.Domain.Interfaces;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Commands;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        // 1. Get existing client
        var client = await _clientRepository.GetByIdAsync(request.Id);
        if (client == null)
            throw new Exception($"Client with ID {request.Id} not found");

        // 2. Check unique constraints (excluding current client)
        var nameExists = await _clientRepository.CompanyNameExistsAsync(
            request.Request.CompanyName, request.Id);
        if (nameExists)
            throw new Exception($"Company '{request.Request.CompanyName}' already exists");

        var emailExists = await _clientRepository.EmailExistsAsync(
            request.Request.Email, request.Id);
        if (emailExists)
            throw new Exception($"Email '{request.Request.Email}' already registered");

        // 3. Update properties
        client.CompanyName = request.Request.CompanyName;
        client.CompanyType = Enum.TryParse<CompanyType>(request.Request.CompanyType, true, out var type) 
            ? type : CompanyType.SME;
        client.Address = request.Request.Address;
        client.Phone = request.Request.Phone;
        client.Email = request.Request.Email;
        client.TaxId = request.Request.TaxId;
        client.Notes = request.Request.Notes;
        client.IsActive = request.Request.IsActive;
        client.UpdatedAt = DateTime.UtcNow;

        // 4. Save changes
        await _clientRepository.UpdateAsync(client);

        // 5. Return DTO
        return _mapper.Map<ClientDto>(client);
    }
}