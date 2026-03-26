using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Infrastructure.Persistence;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class PasswordResetTokenRepository : Repository<PasswordResetToken>, IPasswordResetTokenRepository
{
    public PasswordResetTokenRepository(AppDbContext context) : base(context)
    {
    }

    public Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordResetToken?> GetActiveTokenByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task MarkAsUsedAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task InvalidateUserTokensAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveExpiredTokensAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTokenValidAsync(string token)
    {
        throw new NotImplementedException();
    }
}