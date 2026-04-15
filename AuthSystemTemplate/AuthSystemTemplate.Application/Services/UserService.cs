using AuthSystemTemplate.Application.Common.Results;
using AuthSystemTemplate.Application.DTOs.User;
using AuthSystemTemplate.Application.Interfaces.Repositories;
using AuthSystemTemplate.Application.Interfaces.Services;
using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Domain.Enums;
using AutoMapper;

namespace AuthSystemTemplate.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public UserService(IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IAuthService authService)
    {
        _mapper = mapper;
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

            return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
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

            await _unitOfWork.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(user);
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

    public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            if (users is null)
                return Result<IEnumerable<UserDto>>.Failure(Error.NotFound("No users found"));
            return Result<IEnumerable<UserDto>>.Success(users.Select(u => _mapper.Map<UserDto>(u)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<UserDto?>> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user == null
                ? Result<UserDto?>.Failure(Error.NotFound("User not found"))
                : Result<UserDto?>.Success(_mapper.Map<UserDto>(user));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return Result.Failure(Error.NotFound("User not found"));
            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result> AssignRoleAsync(int userId, Roles role, int assignedByUserId)
    {
        if (await _unitOfWork.Roles.RoleExistsAsync(role))
            return Result.Failure(Error.NotFound("Role does not exist"));

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return Result.Failure(Error.NotFound("User not found"));
        var roleDb = await _unitOfWork.Roles.GetByNameAsync(role);
        if (roleDb == null)
            return Result.Failure(Error.NotFound("Role not found in database"));

        var assignee = await _unitOfWork.Users.GetByIdAsync(assignedByUserId);
        if (assignee == null)
            return Result.Failure(Error.NotFound("Assigner user not found"));

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleDb.Id,
            AssignedBy = assignee
        };
        user.UserRoles.Add(userRole);
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RemoveRoleAsync(int userId, Roles role)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return Result.Failure(Error.NotFound("User not found"));

            var userRole = user.UserRoles.FirstOrDefault(ur => ur.Role.Name == role);
            if (userRole == null)
                return Result.Failure(Error.NotFound("Role not found"));

            user.UserRoles.Remove(userRole);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}