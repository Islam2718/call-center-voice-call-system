using MediatR;
using AutoMapper;
using CallCenterPlatform.Domain.Entities;
using CallCenterPlatform.Domain.Enums;
using CallCenterPlatform.Domain.Interfaces;
using CallCenterPlatform.Application.DTOs;
using CallCenterPlatform.Application.Common.Interfaces;

namespace CallCenterPlatform.Application.Features.Clients.Commands;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateClientCommandHandler(
        IClientRepository clientRepository, 
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate - Company name unique?
        var nameExists = await _clientRepository.CompanyNameExistsAsync(request.Request.CompanyName);
        if (nameExists)
            throw new Exception($"Company '{request.Request.CompanyName}' already exists");

        // 2. Validate - Email unique?
        var emailExists = await _clientRepository.EmailExistsAsync(request.Request.Email);
        if (emailExists)
            throw new Exception($"Email '{request.Request.Email}' already registered");

        // 3. Parse CompanyType from string to enum
        if (!Enum.TryParse<CompanyType>(request.Request.CompanyType, true, out var companyType))
            companyType = CompanyType.SME;  // Default

        // 4. Create Client Entity
        var client = new Client
        {
            Id = Guid.NewGuid(),
            CompanyName = request.Request.CompanyName,
            CompanyType = companyType,
            Address = request.Request.Address,
            Phone = request.Request.Phone,
            Email = request.Request.Email,
            TaxId = request.Request.TaxId,
            Notes = request.Request.Notes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        // 5. Save to database
        await _clientRepository.AddAsync(client);

        // 6. Return DTO
        return _mapper.Map<ClientDto>(client);
    }
}