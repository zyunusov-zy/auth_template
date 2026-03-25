using AuthSystemTemplate.Application.DTOs.User;
using AuthSystemTemplate.Domain.Enums;

namespace AuthSystemTemplate.Application.Interfaces.Services;

public interface IUserService
{
    /// <summary>
    /// Get user profile
    /// FR-5.1: View profile
    /// </summary>
    Task<UserDto?> GetProfileAsync(int userId);
    
    /// <summary>
    /// Update user profile
    /// FR-5.2: Update profile fields
    /// </summary>
    Task<UserDto> UpdateProfileAsync(int userId, UpdateProfileRequest request);
    
    /// <summary>
    /// Change password
    /// FR-5.5: Change password (requires current password verification)
    /// </summary>
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);
    
    /// <summary>
    /// Get all users (Admin only)
    /// FR-4: Role-Based Access Control
    /// </summary>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    
    /// <summary>
    /// Get user by ID (Admin/Manager only)
    /// </summary>
    Task<UserDto?> GetUserByIdAsync(int userId);
    
    /// <summary>
    /// Delete user (Admin only)
    /// </summary>
    Task DeleteUserAsync(int userId);
    
    /// <summary>
    /// Assign role to user (Admin only)
    /// FR-4.6: Default new users to "User" role
    /// </summary>
    Task AssignRoleAsync(int userId, Roles role, int assignedByUserId);
    
    /// <summary>
    /// Remove role from user (Admin only)
    /// </summary>
    Task RemoveRoleAsync(int userId, Roles role);
}