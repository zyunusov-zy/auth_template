using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class PasswordResetTokenRepository : Repository<PasswordResetToken>, IPasswordResetTokenRepository
{
    public PasswordResetTokenRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<PasswordResetToken?> GetActiveTokenByUserIdAsync(int userId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => 
            x.UserId == userId && 
            !x.IsUsed && 
            x.ExpiresAt > DateTime.UtcNow);
    }

    public async Task MarkAsUsedAsync(string token)
    {
        var tokenDb = await _dbSet.FirstOrDefaultAsync(x => x.Token == token);
        if (tokenDb is null) return;
        tokenDb.IsUsed = true;
    }

    public async Task InvalidateUserTokensAsync(int userId)
    {
        var tokens = await _dbSet
            .Where(x => x.UserId == userId && !x.IsUsed)
            .ToListAsync();
        
        foreach (var token in tokens)
            token.IsUsed = true;
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var expiredTokens = await _dbSet
            .Where(x => x.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();
        
        _dbSet.RemoveRange(expiredTokens);
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        return await _dbSet.AnyAsync(x => 
            x.Token == token && 
            !x.IsUsed && 
            x.ExpiresAt > DateTime.UtcNow);
    }
}