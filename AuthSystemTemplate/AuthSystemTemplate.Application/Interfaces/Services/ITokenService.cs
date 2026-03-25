using System.Security.Claims;
using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Interfaces.Services;

public interface ITokenService
{
    /// <summary>
    /// Generate JWT access token
    /// NFR-2: JWT Token Expiration Strategy (15 min)
    /// </summary>
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    
    /// <summary>
    /// Generate refresh token
    /// NFR-2: Refresh Token (7 days)
    /// </summary>
    string GenerateRefreshToken();
    
    /// <summary>
    /// Generate email verification token
    /// FR-1.4: Generate unique email verification token (GUID)
    /// </summary>
    string GenerateEmailVerificationToken();
    
    /// <summary>
    /// Generate password reset token
    /// FR-3.2: Generate secure reset token (cryptographically random)
    /// </summary>
    string GeneratePasswordResetToken();
    
    /// <summary>
    /// Validate and extract user ID from JWT token
    /// </summary>
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    
    /// <summary>
    /// Get user ID from JWT token
    /// </summary>
    int? GetUserIdFromToken(string token);
}