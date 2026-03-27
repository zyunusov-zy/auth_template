namespace AuthSystemTemplate.Domain.Entities;

public class EmailVerificationToken : BaseEntity
{
    public int UserId { get; set; }

    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool IsUsed { get; set; }

    public User User { get; set; } = null!;
}