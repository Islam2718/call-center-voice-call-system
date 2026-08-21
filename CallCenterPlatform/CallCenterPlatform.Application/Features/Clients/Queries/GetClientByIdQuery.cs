using MediatR;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Queries;

public record GetClientByIdQuery(Guid Id) : IRequest<ClientDto>;