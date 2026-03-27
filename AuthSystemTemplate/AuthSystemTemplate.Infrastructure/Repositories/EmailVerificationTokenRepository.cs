using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Infrastructure.Persistence;

namespace AuthSystemTemplate.Infrastructure.Repositories;

public class EmailVerificationTokenRepository : Repository<EmailVerificationToken>
{
    public EmailVerificationTokenRepository(AppDbContext context) : base(context)
    {
    }
    
}