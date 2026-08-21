using MediatR;

namespace CallCenterPlatform.Application.Features.Clients.Commands;

public record DeleteClientCommand(Guid Id) : IRequest<bool>;
public record SoftDeleteClientCommand(Guid Id) : IRequest<bool>;