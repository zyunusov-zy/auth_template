using AuthSystemTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthSystemTemplate.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Role => Set<Role>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();
    public DbSet<PasswordResetToken> PasswordResetToken => Set<PasswordResetToken>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
