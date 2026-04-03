using AuthSystemTemplate.Domain.Entities;

namespace AuthSystemTemplate.Application.Interfaces.Repositories;

public interface IEmailVerificationTokenRepository : IRepository<EmailVerificationToken>
{

}