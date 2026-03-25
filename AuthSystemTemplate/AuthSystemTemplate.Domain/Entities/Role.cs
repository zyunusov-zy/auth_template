using AuthSystemTemplate.Domain.Enums;

namespace AuthSystemTemplate.Domain.Entities;

public class Role : BaseEntity
{
    public Roles Name { get; set; } = Roles.User;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}