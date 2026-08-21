using MediatR;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Commands;

public record UpdateClientCommand(Guid Id, UpdateClientRequestDto Request) : IRequest<ClientDto>;