namespace AuthSystemTemplate.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IPasswordResetTokenRepository PasswordResetTokens { get; }
    
    /// <summary>
    /// Save all changes to database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begin database transaction
    /// </summary>
    Task BeginTransactionAsync();
    
    /// <summary>
    /// Commit transaction
    /// </summary>
    Task CommitTransactionAsync();
    
    /// <summary>
    /// Rollback transaction
    /// </summary>
    Task RollbackTransactionAsync();
}