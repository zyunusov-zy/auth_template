using System.ComponentModel.DataAnnotations;

namespace AuthSystemTemplate.Domain.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}