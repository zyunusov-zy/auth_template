using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Get user by email with roles loaded
    /// </summary>
    Task<User?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Get user by ID with roles and tokens loaded
    /// </summary>
    Task<User?> GetByIdWithDetailsAsync(int id);
    
    /// <summary>
    /// Check if email already exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email);
    
    /// <summary>
    /// Get all users with their roles
    /// </summary>
    Task<IEnumerable<User>> GetAllWithRolesAsync();
    
    /// <summary>
    /// Search users by name or email
    /// </summary>
    Task<IEnumerable<User>> SearchAsync(string searchTerm);
}