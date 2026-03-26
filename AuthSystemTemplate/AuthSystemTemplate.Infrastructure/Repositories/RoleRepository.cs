using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Domain.Enums;
using AuthSystemTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(Roles roleName)
    {
        return await _dbSet.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Name == roleName);
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
    {
        return await _dbSet.Where(r => r.UserRoles.Any(ur => ur.UserId == userId)).ToListAsync();
    }

    public async Task<bool> RoleExistsAsync(Roles roleName)
    {
        return await _dbSet.AnyAsync(r => r.Name == roleName);
    }
}