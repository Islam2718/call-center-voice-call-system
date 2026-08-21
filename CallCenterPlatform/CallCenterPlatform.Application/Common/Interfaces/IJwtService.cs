using CallCenterPlatform.Domain.Entities;

namespace CallCenterPlatform.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}