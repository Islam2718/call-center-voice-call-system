using MediatR;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Auth.Commands;

public record RegisterCommand(RegisterRequestDto Request) : IRequest<AuthResponseDto>;