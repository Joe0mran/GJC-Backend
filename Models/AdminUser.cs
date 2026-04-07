using System.ComponentModel.DataAnnotations;

namespace GJC.Models;
public class AdminUser
{
    [Key] public int AdminUserId { get; set; }
    [Required] public string Email { get; set; } = default!;
    [Required] public string FirstName { get; set; } = default!;
    [Required] public string LastName { get; set; } = default!;
    [Required] public string PasswordHash { get; set; } = default!;
    [Required] public string Role { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}