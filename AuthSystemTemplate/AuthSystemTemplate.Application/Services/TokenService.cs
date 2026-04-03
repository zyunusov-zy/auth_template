using System.Security.Claims;
using AuthSystemTemplate.Application.Interfaces.Services;
using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Services;

public class TokenService : ITokenService
{
    public string GenerateAccessToken(User user, IEnumerable<string> roles)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public string GenerateEmailVerificationToken()
    {
        throw new NotImplementedException();
    }

    public string GeneratePasswordResetToken()
    {
        throw new NotImplementedException();
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        throw new NotImplementedException();
    }

    public int? GetUserIdFromToken(string token)
    {
        throw new NotImplementedException();
    }
}