namespace AuthSystemTemplate.Domain.Entities;

public class PasswordResetToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}