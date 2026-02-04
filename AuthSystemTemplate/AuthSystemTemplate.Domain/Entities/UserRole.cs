namespace AuthSystemTemplate.Domain.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public int? AssignedUserId { get; set; }
    public User? AssignedBy { get; set; } = null!;
}