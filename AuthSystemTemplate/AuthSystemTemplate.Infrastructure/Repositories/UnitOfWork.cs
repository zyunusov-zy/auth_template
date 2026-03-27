using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Infrastructure.Persistence;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IRefreshTokenRepository? _refreshTokens;
    private IPasswordResetTokenRepository? _passwordResetTokens;
    private EmailVerificationTokenRepository? _emailVerificationToken;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    public IUserRepository Users => 
        _users ??= new UserRepository(_context);
    
    public IRoleRepository Roles => 
        _roles ??= new RoleRepository(_context);
    
    public IRefreshTokenRepository RefreshTokens => 
        _refreshTokens ??= new RefreshTokenRepository(_context);
    
    public IPasswordResetTokenRepository PasswordResetTokens => 
        _passwordResetTokens ??= new PasswordResetTokenRepository(_context);
    
    public EmailVerificationTokenRepository EmailVerificationTokens => 
        _emailVerificationToken ??= new EmailVerificationTokenRepository(_context);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}