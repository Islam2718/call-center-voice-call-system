using MediatR;
using CallCenterPlatform.Domain.Entities;
using CallCenterPlatform.Domain.Enums;
using CallCenterPlatform.Domain.Interfaces;
using CallCenterPlatform.Application.Common.Interfaces;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var emailExists = await _userRepository.EmailExistsAsync(request.Request.Email);
        if (emailExists)
            throw new Exception("Email already registered");

        // Parse role
        if (!Enum.TryParse<UserRole>(request.Request.Role, true, out var role))
            role = UserRole.Agent;

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password),
            FullName = request.Request.FullName,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }
}