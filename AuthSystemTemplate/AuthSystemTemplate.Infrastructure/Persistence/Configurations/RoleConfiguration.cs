using AuthSystemTemplate.Domain.Entities;
using AuthSystemTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthSystemTemplate.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasConversion<string>().HasDefaultValue(Roles.User).HasSentinel(Roles.User);

        builder.HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasData(
            new Role { Id = 1, Name = Roles.User },
            new Role { Id = 2, Name = Roles.Manager },
            new Role { Id = 3, Name = Roles.Admin }
        );
    }
}