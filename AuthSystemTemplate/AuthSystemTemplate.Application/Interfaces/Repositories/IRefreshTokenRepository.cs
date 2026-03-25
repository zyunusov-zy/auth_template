using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    /// <summary>
    /// Get refresh token with user details
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token);
    
    /// <summary>
    /// Get all active (non-revoked, non-expired) tokens for a user
    /// </summary>
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
    
    /// <summary>
    /// Revoke all tokens for a user
    /// </summary>
    Task RevokeAllUserTokensAsync(int userId);
    
    /// <summary>
    /// Remove expired tokens (for cleanup job)
    /// </summary>
    Task RemoveExpiredTokensAsync();
    
    /// <summary>
    /// Check if token is valid (exists, not revoked, not expired)
    /// </summary>
    Task<bool> IsTokenValidAsync(string token);
}