using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.src.Persistance.Model;

[Index(nameof(Email), IsUnique = true)]
public class Customer
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(256)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(256)]
    public string LastName { get; set; } = null!;
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    [MaxLength(256)]
    public string? Street { get; private set; }
    [MaxLength(256)]
    public string? HouseNumber { get; private set; }
    [MaxLength(256)]
    public string? ZipCode { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}