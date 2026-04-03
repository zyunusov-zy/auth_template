using AuthSystemTemplate.Application.DTOs.User;
using AuthSystemTemplate.Application.Interfaces.Services;
using AuthSystemTemplate.Domain.Enums;

namespace AuthSystemTemplate.Application.Services;

public class UserService : IUserService
{
    public Task<UserDto?> GetProfileAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateProfileAsync(int userId, UpdateProfileRequest request)
    {
        throw new NotImplementedException();
    }

    public Task ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserDto?> GetUserByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task AssignRoleAsync(int userId, Roles role, int assignedByUserId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRoleAsync(int userId, Roles role)
    {
        throw new NotImplementedException();
    }
}