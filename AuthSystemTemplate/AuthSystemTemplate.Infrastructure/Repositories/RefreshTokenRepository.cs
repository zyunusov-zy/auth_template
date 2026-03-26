using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId)
    {
        return await _dbSet.Where(t => t.UserId == userId
                                       && !t.IsRevoked
                                       && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task RevokeAllUserTokensAsync(int userId)
    {
        var activeTokens = await _dbSet.Where(t => t.UserId == userId && !t.IsRevoked).ToListAsync();

        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;
        }
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var expiryTokens = await _dbSet.Where(t => t.ExpiresAt < DateTime.UtcNow).ToListAsync();

        _dbSet.RemoveRange(expiryTokens);
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        var tokenV = await _dbSet.FirstOrDefaultAsync(t => t.Token == token);
        if (tokenV == null)
            return false;
        return !tokenV.IsRevoked && tokenV.ExpiresAt >= DateTime.UtcNow;
    }
}