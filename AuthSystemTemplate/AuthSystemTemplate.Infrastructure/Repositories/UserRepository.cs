using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Include(u => u.RefreshTokens)
            .Include(u => u.PasswordResetTokens).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllWithRolesAsync()
    {
        return await _dbSet.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
    }

    public async Task<IEnumerable<User>> SearchAsync(string searchTerm)
    {
        return await _dbSet.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(u =>
                u.Email.Contains(searchTerm) || u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm))
            .ToListAsync();
    }
}