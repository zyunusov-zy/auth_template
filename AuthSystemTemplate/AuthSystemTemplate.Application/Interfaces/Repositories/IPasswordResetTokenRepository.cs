using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Interfaces.Repositories;

public interface IPasswordResetTokenRepository : IRepository<PasswordResetToken>
{
    /// <summary>
    /// Get reset token with user details
    /// </summary>
    Task<PasswordResetToken?> GetByTokenAsync(string token);
    
    /// <summary>
    /// Get active (unused, non-expired) reset token for user
    /// </summary>
    Task<PasswordResetToken?> GetActiveTokenByUserIdAsync(int userId);
    
    /// <summary>
    /// Mark token as used
    /// </summary>
    Task MarkAsUsedAsync(string token);
    
    /// <summary>
    /// Invalidate all reset tokens for a user
    /// </summary>
    Task InvalidateUserTokensAsync(int userId);
    
    /// <summary>
    /// Remove expired tokens (cleanup)
    /// </summary>
    Task RemoveExpiredTokensAsync();
    
    /// <summary>
    /// Check if token is valid (exists, not used, not expired)
    /// </summary>
    Task<bool> IsTokenValidAsync(string token);
}