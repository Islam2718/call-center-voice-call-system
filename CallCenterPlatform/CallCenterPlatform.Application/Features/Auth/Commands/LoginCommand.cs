using MediatR;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequestDto Request) : IRequest<AuthResponseDto>;