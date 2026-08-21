using MediatR;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Commands;

public record CreateClientCommand(CreateClientRequestDto Request) : IRequest<ClientDto>;