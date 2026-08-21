using MediatR;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Clients.Queries;

public record GetAllClientsQuery(bool IncludeInactive = false) : IRequest<IEnumerable<ClientDto>>;