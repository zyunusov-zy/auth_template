namespace AuthSystemTemplate.Domain.Entities;

public class PasswordResetToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UsedAt { get; set; }
    public bool IsUsed { get; set; } = false;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}