using MediatR;
using CallCenterPlatform.Domain.Interfaces;
using CallCenterPlatform.Application.Common.Interfaces;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Request.Email);
        if (user == null)
            throw new Exception("Invalid credentials");

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Request.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        if (!user.IsActive)
            throw new Exception("Account is deactivated");

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Generate token
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