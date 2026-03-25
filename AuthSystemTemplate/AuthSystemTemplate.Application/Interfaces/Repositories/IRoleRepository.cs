using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Domain.Enums;

namespace AuthSystemTemplate.Application.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Get role by name (enum)
    /// </summary>
    Task<Role?> GetByNameAsync(Roles roleName);
    
    /// <summary>
    /// Get all roles for a specific user
    /// </summary>
    Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
    
    /// <summary>
    /// Check if role exists
    /// </summary>
    Task<bool> RoleExistsAsync(Roles roleName);
}
