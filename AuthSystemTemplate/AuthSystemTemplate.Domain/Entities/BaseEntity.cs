using System.ComponentModel.DataAnnotations;

namespace AuthSystemTemplate.Domain.Entities;

public class BaseEntity
{
    [Key]
    public long Id { get; set; }
}