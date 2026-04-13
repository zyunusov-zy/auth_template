using AuthSystemTemplate.Application.Common.Results;
using AuthSystemTemplate.Application.DTOs.User;
using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Application.Interfaces.Services;
using AuthSystemTemplate.Domain.Enums;

namespace AuthSystemTemplate.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;

    public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _authService = authService;
    }

    public async Task<Result<UserDto>> GetProfileAsync(int userId)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdWithDetailsAsync(userId);
            if (user is null)
                return Result<UserDto>.Failure(Error.NotFound("User not found"));

            var roles = await _unitOfWork.Roles.GetUserRolesAsync(userId);
            var roleNames = roles.Select(x => x.Name.ToString()).ToList();

            var userDto = new UserDto
            (
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                roleNames,
                user.EmailVerified,
                user.CreatedAt
            );
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<UserDto>> UpdateProfileAsync(int userId, UpdateProfileRequest request)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdWithDetailsAsync(userId);
            if (user is null)
                return Result<UserDto>.Failure(Error.NotFound("User not found"));
            if (!string.IsNullOrWhiteSpace(request.FirstName))
                user.FirstName = request.FirstName;
            if (!string.IsNullOrWhiteSpace(request.LastName))
                user.LastName = request.LastName;
            await _unitOfWork.Users.UpdateAsync(user);
            var roles = await _unitOfWork.Roles.GetUserRolesAsync(userId);
            var roleNames = roles.Select(x => x.Name.ToString()).ToList();


            await _unitOfWork.SaveChangesAsync();
            var userDto = new UserDto
            (
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                roleNames,
                user.EmailVerified,
                user.CreatedAt
            );
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdWithDetailsAsync(userId);
            if (user is null)
                return Result.Failure(Error.NotFound("User not found"));
            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return Result.Failure(Error.Validation("Invalid current password"));
            if (!_passwordHasher.IsPasswordStrong(request.NewPassword))
                return Result.Failure(Error.Validation("Password is not strong"));
            var isSamePassword =
                _passwordHasher.VerifyPassword(request.NewPassword, user.PasswordHash, user.PasswordSalt);

            if (isSamePassword)
                return Result.Failure(Error.Validation("New password must be different from current password"));

            var (hash, salt) = _passwordHasher.HashPassword(request.NewPassword);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            await _authService.LogoutAllDevicesAsync(user.Id);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<UserDto?>> GetUserByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> AssignRoleAsync(int userId, Roles role, int assignedByUserId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveRoleAsync(int userId, Roles role)
    {
        throw new NotImplementedException();
    }
}